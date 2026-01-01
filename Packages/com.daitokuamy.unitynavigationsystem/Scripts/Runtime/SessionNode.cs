namespace UnityNavigationSystem {
    /// <summary>
    /// SessionとなるNavNode
    /// ※Root直下にのみ置けるNode
    /// </summary>
    public abstract class SessionNode : NavNode, ISessionNode {
        /// <inheritdoc/>
        protected sealed override bool CanAddNode(INavNode node) {
            if (node is ScreenNode) {
                return true;
            }

            return false;
        }
    }
}