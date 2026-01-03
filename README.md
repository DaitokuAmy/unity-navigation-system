# unity-navigation-system
Unityで画面遷移をコントロールするシステム
## 概要
#### 特徴
* 構成図をプログラム的に定義し、ライフサイクルのネスト管理を実現
* ルーター機能を追加する事で、Stack管理やTree管理といった遷移図的なコントロールも可能
* 抽象化された遷移演出を差し込む事で、安定的な画面遷移時の演出表現を実現
## セットアップ
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
        "com.daitokuamy.gameaibehaviour": "https://github.com/DaitokuAmy/unity-navigation-system.git?path=/Packages/com.daitokuamy.unitynavigationsystem"
    }
}
```
バージョンを指定したい場合には以下のように記述します。  
https://github.com/DaitokuAmy/unity-navigation-system.git?path=/Packages/com.daitokuamy.unitynavigationsystem#1.0.0
