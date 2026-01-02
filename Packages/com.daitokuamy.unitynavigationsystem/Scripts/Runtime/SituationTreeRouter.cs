using System;

namespace UnityNavigationSystem {
    /// <summary>
    /// NavNode遷移に使うツリー
    /// </summary>
    public sealed class NavNodeTreeRouter : StateTreeRouter<Type, INavNode, NavNodeTree.TransitionOption> {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NavNodeTreeRouter(NavNodeTree container)
            : base(container) {
        }
    }
}