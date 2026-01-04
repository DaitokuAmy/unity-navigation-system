namespace UnityNavigationSystem {
    /// <summary>
    /// RootとなるNavNode
    /// </summary>
    public abstract class RootNode : NavNode, IRootNode {
        /// <inheritdoc/>
        protected override bool IsParallelLoading => false;
    }
}