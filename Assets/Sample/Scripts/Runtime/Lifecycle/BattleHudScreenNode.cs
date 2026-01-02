using UnityNavigationSystem;

namespace Sample {
    /// <summary>
    /// BattleHud用のScreenNode
    /// </summary>
    public sealed class BattleHudScreenNode : ScreenNode<BattleHudUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "BattleHud";

        /// <inheritdoc/>
        protected override void Activate(TransitionHandle<INavNode> handle, IScope scope) {
            base.Activate(handle, scope);
            
            UIComponent.fullScreenButton.onClick.AddListener(() => {
                Engine.TransitionTo<BattleDialogScreenNode>(null, new OutInTransition());
            });
            
            UIComponent.backButton.onClick.AddListener(() => {
                Engine.Back(new OutInTransition());
            });
        }

        /// <inheritdoc/>
        protected override void Deactivate(TransitionHandle<INavNode> handle) {
            UIComponent.fullScreenButton.onClick.RemoveAllListeners();
            UIComponent.backButton.onClick.RemoveAllListeners();
            
            base.Deactivate(handle);
        }
    }
}