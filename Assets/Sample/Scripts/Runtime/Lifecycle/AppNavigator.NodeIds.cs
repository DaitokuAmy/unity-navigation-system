using UnityNavigationSystem;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator {
        /// <summary>
        /// 登録Id
        /// </summary>
        public static class Id {
            public const int Invalid = NavigationEngine.InvalidNodeId;
            
            public const int Root = 1;
            
            public const int Title = 100;
            public const int TitleTop = 101;
            
            public const int OutGame = 200;
            public const int Home = 201;
            public const int HomeTop = 202;
            public const int PartyTop = 204;
            public const int GachaTop = 205;
            public const int HomeShopOnTop = 250;
            public const int HomeShopOnParty = 251;
            public const int HomeShopOnGacha = 252;
            
            public const int Battle = 300;
            public const int BattleHud = 301;
            public const int BattlePause = 302;
        }
    }
}