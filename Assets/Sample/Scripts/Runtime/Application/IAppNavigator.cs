namespace Sample.Application {
    /// <summary>
    /// アプリの遷移操作用インターフェース
    /// </summary>
    public partial interface IAppNavigator {
        /// <summary>
        /// 戻る遷移
        /// </summary>
        void Back(int depth = 1);
    }
}
