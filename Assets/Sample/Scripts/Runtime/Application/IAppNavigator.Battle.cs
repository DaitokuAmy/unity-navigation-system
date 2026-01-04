namespace Sample.Application {
    /// <summary>
    /// アプリの遷移操作用インターフェース
    /// </summary>
    partial interface IAppNavigator {
        /// <summary>
        /// バトルへの遷移
        /// </summary>
        void GoToBattle();
        
        /// <summary>
        /// バトル中ポーズメニューへの遷移
        /// </summary>
        void GoToBattlePause();
    }
}
