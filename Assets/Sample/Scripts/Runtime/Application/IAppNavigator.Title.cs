namespace Sample.Application {
    /// <summary>
    /// アプリの遷移操作用インターフェース
    /// </summary>
    partial interface IAppNavigator {
        /// <summary>
        /// タイトル画面への遷移
        /// </summary>
        void GoToTitle();
    }
}
