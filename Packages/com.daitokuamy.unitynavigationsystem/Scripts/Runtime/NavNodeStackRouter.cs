using System;

namespace UnityNavigationSystem {
    /// <summary>
    /// NavigationNode遷移に使うスタック
    /// </summary>
    public sealed class NavNodeStackRouter : StateStackRouter<Type, INavNode, NavNodeTree.TransitionOption>, INavNodeStateRouter {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NavNodeStackRouter(NavNodeTree container)
            : base(container) {
        }
    }
}