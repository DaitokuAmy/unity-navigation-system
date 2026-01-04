using Sample.Application;
using UnityNavigationSystem;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    public partial class AppNavigator : IAppNavigator {
        /// <summary>OutIn遷移</summary>
        private static readonly ITransition OutInTransition = new OutInTransition();
        /// <summary>Cross遷移</summary>
        private static readonly ITransition CrossTransition = new CrossTransition();
        
        private NavigationEngine _engine;

        /// <summary>
        /// 戻る遷移
        /// </summary>
        void IAppNavigator.Back(int depth) {
            _engine.Back(depth, null, OutInTransition);
        }
        
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="navigationEngine">遷移に利用するEngine</param>
        public void Initialize(NavigationEngine navigationEngine) {
            _engine = navigationEngine;
        }
    }
}