using UnityNavigationSystem;

namespace Sample {
    /// <summary>
    /// HomeTop用のScreenNode
    /// </summary>
    public sealed class HomeTopScreenNode : ScreenNode<HomeTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "HomeTop";

        /// <inheritdoc/>
        protected override void Activate(TransitionHandle<INavNode> handle, IScope scope) {
            base.Activate(handle, scope);
            
            UIComponent.battleButton.onClick.AddListener(() => {
                Engine.TransitionTo<BattleHudScreenNode>(null, new OutInTransition());
            });
        }

        /// <inheritdoc/>
        protected override void Deactivate(TransitionHandle<INavNode> handle) {
            UIComponent.battleButton.onClick.RemoveAllListeners();
            
            base.Deactivate(handle);
        }
    }
}