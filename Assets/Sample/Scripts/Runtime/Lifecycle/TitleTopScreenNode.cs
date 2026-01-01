using UnityNavigationSystem;

namespace Sample {
    /// <summary>
    /// TitleTop用のScreenNode
    /// </summary>
    public sealed class TitleTopScreenNode : ScreenNode<TitleTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "TitleTop";

        /// <inheritdoc/>
        protected override void Activate(TransitionHandle<INavNode> handle, IScope scope) {
            base.Activate(handle, scope);
            
            UIComponent.startButton.onClick.AddListener(() => {
                Engine.TransitionTo<BattleHudScreenNode>(null, new OutInTransition());
            });
        }

        /// <inheritdoc/>
        protected override void Deactivate(TransitionHandle<INavNode> handle) {
            UIComponent.startButton.onClick.RemoveAllListeners();
            
            base.Deactivate(handle);
        }
    }
}