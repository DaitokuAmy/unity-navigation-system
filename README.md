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
    .Build();
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
```csharp
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
                    homeTop.SetShortcutScope(homeTop);
                    homeTop.Connect(typeof(GachaTopScreenNode), gachaTop => {
                            gachaTop.SetShortcutScope(homeTop);
                        })
                        .Connect(typeof(PartyTopScreenNode), partyTop => {
                            partyTop.SetShortcutScope(homeTop);
                        })
                        .Connect(typeof(BattleHudScreenNode), battleHud => {
                            battleHud.Connect(typeof(BattlePauseScreenNode))
                                .SetGlobalShortcut();
                        });
                });
            })
            .Build(router);
        return router;
    })
    .Build();
```
この定義をしておくと、以下のようにBack関数が使えるようになり、戻り先のEngine解決が可能になります
```csharp
_navigationEngine.Back(new OutInTransition(), new LoadingEffect());
```
ちなみに、以下のように記述する事でStack管理による戻り先コントロールも可能です
```csharp
_navigationEngine = NavigationEngineBuilder.Create()
    .CreateTree(new RootNode(), root => {
        /* 省略 */
    })
    .CreateRouter(tree => new NavNodeStackRouter(tree))
    .Build();
```
#### NavNode
基本的に、アプリ内で定義するNodeは以下のNodeを継承して作成します
* **[RootNode](https://github.com/DaitokuAmy/unity-navigation-system/blob/main/Packages/com.daitokuamy.unitynavigationsystem/Scripts/Runtime/Node/RootNode.cs)**
  * 遷移に必ず1つ存在する常駐Node
* **[SessionNode](https://github.com/DaitokuAmy/unity-navigation-system/blob/main/Packages/com.daitokuamy.unitynavigationsystem/Scripts/Runtime/Node/SessionNode.cs)**
  * RootNodeにのみぶら下げる事が可能なNode、用途的にはシステム単位のライフサイクル単位で利用（開くアニメーションなどがない）
* **[ScreenNode](https://github.com/DaitokuAmy/unity-navigation-system/blob/main/Packages/com.daitokuamy.unitynavigationsystem/Scripts/Runtime/Node/ScreenNode.cs)**
  * SessionNode or ScreenNodeにぶら下げる事が可能なNode、UIや画面単位で作成する事を想定（開く、閉じるアニメーションの記述が可能）

具体的に、実際にNodeに記述可能なライフサイクルイベントは以下になります  
* **Standby**
  * エンジンによってNodeが追加された時に呼び出されます
* **Load**
  * PreLoadを含む、遷移時の読み込み処理が実行された時に呼び出されます
  * 有効になるNodeを階層的に並列実行してLoadを呼び出します
* **Initialize**
  * 遷移時の読み込み処理が終わった後に呼び出されます
* **PreOpen/Open/PostOpen** ※ScreenNodeのみ
  * Initializeが終わった後に呼び出されます
* **Activate**
  * PreOpen/Open/PostOpenが終わった後に呼び出されます
* **Deactivate**
  * Activateの対
  * 他Nodeに遷移された際、閉じるアニメーションの前に呼び出されます
* **PreClose/Close/PostClose** ※ScreenNodeのみ
  * Deactivateされた後に、閉じるアニメーション記述用として呼び出されます
* **Terminate**
  * Initializeの対
  * 閉じるアニメーションの後に呼び出されます
* **Unload**
  * Loadの対
  * Terminateの後に呼び出されますが、PreLoadされている場合は呼び出されません
* **Release**
  * Standbyの対
  * エンジンからNodeが除外された時に呼び出されます
