namespace Sample.Application {
    /// <summary>
    /// アプリの遷移操作用インターフェース
    /// </summary>
    partial interface IAppNavigator {
        /// <summary>
        /// ホーム画面への遷移
        /// </summary>
        void GoToHome();

        /// <summary>
        /// ガチャ画面への遷移
        /// </summary>
        void GoToGacha();

        /// <summary>
        /// パーティ画面への遷移
        /// </summary>
        void GoToParty();
    }
}
