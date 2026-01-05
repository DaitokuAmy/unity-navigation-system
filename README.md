# unity-navigation-system
Unityで画面遷移をコントロールするシステム
## 背景
* Unityシーンだけではライフサイクル制御が大味になってしまうため、階層的にライフサイクルを制御でき、コード記述箇所を明確にする環境が必要だった
* アウトゲームもインゲームもなるべく同じ機構で動かしたく、遷移時の演出（ローディングなど）も強引な記述方法をしなくて済むようにしたかった
* 戻り先を自前で管理するのではなく、システム側で良い感じにコントロールをしてほしかった（Router機能）
## 概要
#### 特徴
* 構成図(NavNodeTree)をプログラム的に定義し、ネスト可能なライフサイクル管理を実現
* Router機能を追加する事で、Stack管理やTree管理といった戻り先の自動制御を実現
* 抽象化された遷移方法や演出を差し込む事で、安定的に画面遷移方法や演出実装を実現
## セットアップ
#### 依存アセットインストール
本PackageはVContainerを利用しているため、事前にVContainerのインストールが必要です  
[こちら](https://vcontainer.hadashikick.jp/ja/getting-started/installation)を参考にしてインストールをしてください  

#### インストール
1. Window > Package ManagerからPackage Managerを開く
2. 「+」ボタン > Add package from git URL
3. 以下を入力してインストール
   * https://github.com/DaitokuAmy/unity-navigation-system.git?path=/Packages/com.daitokuamy.unitynavigationsystem
   ![image](https://user-images.githubusercontent.com/6957962/209446846-c9b35922-d8cb-4ba3-961b-52a81515c808.png)

あるいはPackages/manifest.jsonを開き、dependenciesブロックに以下を追記します。
```json
{
    "dependencies": {
        "com.daitokuamy.unitynavigationsystem": "https://github.com/DaitokuAmy/unity-navigation-system.git?path=/Packages/com.daitokuamy.unitynavigationsystem"
    }
}
```
バージョンを指定したい場合には以下のように記述します。  
https://github.com/DaitokuAmy/unity-navigation-system.git?path=/Packages/com.daitokuamy.unitynavigationsystem#1.0.0

## 機能説明
#### NavigationEngine
以下のように、NavigationEngineBuilderを使ってEngineを生成し...
```csharp
_navigationEngine = NavigationEngineBuilder.Create()
    .CreateTree(new RootNode(), root => {
        root.AddSession(new TitleSessionNode(), title => {
                title.AddScreen(new TitleTopScreenNode());
            })
            .AddSession(new BattleSessionNode(), battle => {
                battle.AddScreen(new BattleHudScreenNode(), battleHud => {
                    battleHud.AddScreen(new BattlePauseScreenNode());
                });
            });
    })
    .Build(_rootResolver);
```
以下のようにそれぞれのNode間を遷移する事ができます
```csharp
var handle = _navigationEngine.TransitionTo<TitleTopScreenNode>(node => {
        /* nodeにパラメータ渡したり出来る */
    },
    new OutInTransition(),
    new LoadingEffect());

// 遷移完了待ち
await handle;
```
#### NavNodeTreeRouter
ライフサイクル構造もツリーなのでややこしいですが、以下のような記述を加える事で明示的な遷移ツリーを定義する事ができます
```
_navigationEngine = NavigationEngineBuilder.Create()
    .CreateTree(new RootNode(), root => {
        /* 省略 */
    })
    .CreateRouter(tree => {
        // ツリー構造で遷移図を構築
        var router = new NavNodeTreeRouter(tree);
        NavNodeTreeRouterBuilder.Create()
            .AddRoot(typeof(TitleTopScreenNode), titleTop => {
                titleTop.Connect(typeof(HomeTopScreenNode), homeTop => {
                    homeTop.SetFallback(homeTop);
                    homeTop.Connect(typeof(GachaTopScreenNode), gachaTop => {
                            gachaTop.SetFallback(homeTop);
                        })
                        .Connect(typeof(PartyTopScreenNode), partyTop => {
                            partyTop.SetFallback(homeTop);
                        })
                        .Connect(typeof(BattleHudScreenNode), battleHud => {
                            battleHud.Connect(typeof(BattlePauseScreenNode))
                                .SetFallback();
                        });
                });
            })
            .Build(router);
        return router;
    })
    .Build(_rootResolver);
```
この定義をしておくと、以下のようにBack関数が使えるようになり、戻り先のEngine解決が可能になります
```
_navigationEngine.Back(new OutInTransition(), new LoadingEffect());
```
