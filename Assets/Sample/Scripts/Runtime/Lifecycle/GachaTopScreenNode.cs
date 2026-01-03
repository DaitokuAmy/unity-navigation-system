using UnityNavigationSystem;

namespace Sample {
    /// <summary>
    /// GachaTop用のScreenNode
    /// </summary>
    public sealed class GachaTopScreenNode : ScreenNode<GachaTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "GachaTop";

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