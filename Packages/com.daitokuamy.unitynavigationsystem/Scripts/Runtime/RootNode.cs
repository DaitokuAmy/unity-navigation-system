namespace UnityNavigationSystem {
    /// <summary>
    /// RootとなるNavNode
    /// </summary>
    public abstract class RootNode : NavNode, IRootNode {
        /// <inheritdoc/>
        protected sealed override bool CanAddNode(INavNode node) {
            if (node is SessionNode) {
                return true;
            }

            return false;
        }
    }
}