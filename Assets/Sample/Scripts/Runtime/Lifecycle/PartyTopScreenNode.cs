using UnityNavigationSystem;

namespace Sample {
    /// <summary>
    /// PartyTop用のScreenNode
    /// </summary>
    public sealed class PartyTopScreenNode : ScreenNode<PartyTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "PartyTop";

        /// <inheritdoc/>
        protected override void Activate(TransitionHandle<INavNode> handle, IScope scope) {
            base.Activate(handle, scope);
        }

        /// <inheritdoc/>
        protected override void Deactivate(TransitionHandle<INavNode> handle) {
            base.Deactivate(handle);
        }
    }
}