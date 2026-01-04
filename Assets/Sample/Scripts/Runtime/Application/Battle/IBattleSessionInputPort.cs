namespace Sample.Application {
    /// <summary>
    /// バトルセッション用インターフェース
    /// </summary>
    public interface IBattleSessionInputPort {
        /// <summary>
        /// 開始
        /// </summary>
        void Start();

        /// <summary>
        /// 終了
        /// </summary>
        void Exit();
    }
}