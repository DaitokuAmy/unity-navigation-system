namespace UnityNavigationSystem {
    /// <summary>
    /// NavNodeの状態定義
    /// </summary>
    public enum NavNodePhaseType {
        /// <summary>未登録</summary>
        Unknown = -1,
        /// <summary>待機</summary>
        Standby,
        /// <summary>読み込み</summary>
        Load,
        /// <summary>初期化</summary>
        Initialize,
        /// <summary>開くアニメ再生</summary>
        Open,
        /// <summary>アクティブ化</summary>
        Activate,
        /// <summary>非アクティブ化</summary>
        Deactivate,
        /// <summary>閉じるアニメ再生</summary>
        Close,
        /// <summary>終了</summary>
        Terminate,
        /// <summary>アンロード</summary>
        Unload,
        /// <summary>廃棄</summary>
        Release,
    }
}