using UnityNavigationSystem;

namespace Sample {
    /// <summary>
    /// BattleDialog用のScreenNode
    /// </summary>
    public sealed class BattleDialogScreenNode : ScreenNode<BattleDialogUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "BattleDialog";

        /// <inheritdoc/>
        protected override void Activate(TransitionHandle<INavNode> handle, IScope scope) {
            base.Activate(handle, scope);
            
            UIComponent.cancelButton.onClick.AddListener(() => {
                Engine.Back(new OutInTransition());
            });
            
            UIComponent.decideButton.onClick.AddListener(() => {
                Engine.TransitionTo<TitleTopScreenNode>(new OutInTransition());
            });
        }

        /// <inheritdoc/>
        protected override void Deactivate(TransitionHandle<INavNode> handle) {
            UIComponent.cancelButton.onClick.RemoveAllListeners();
            UIComponent.decideButton.onClick.RemoveAllListeners();
            
            base.Deactivate(handle);
        }
    }
}