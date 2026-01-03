using UnityNavigationSystem;

namespace Sample {
    /// <summary>
    /// Home共通用のScreenNode
    /// </summary>
    public sealed class HomeScreenNode : ScreenNode<HomeUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "Home";

        /// <inheritdoc/>
        protected override void Activate(TransitionHandle<INavNode> handle, IScope scope) {
            base.Activate(handle, scope);
            
            UIComponent.homeButton.onClick.AddListener(() => {
                Engine.TransitionTo<HomeTopScreenNode>(null, new OutInTransition());
            });
            
            UIComponent.gachaButton.onClick.AddListener(() => {
                Engine.TransitionTo<GachaTopScreenNode>(null, new OutInTransition());
            });
            
            UIComponent.partyButton.onClick.AddListener(() => {
                Engine.TransitionTo<PartyTopScreenNode>(null, new OutInTransition());
            });
            
            UIComponent.backButton.onClick.AddListener(() => {
                Engine.Back(new OutInTransition());
            });
        }

        /// <inheritdoc/>
        protected override void Deactivate(TransitionHandle<INavNode> handle) {
            UIComponent.homeButton.onClick.RemoveAllListeners();
            UIComponent.gachaButton.onClick.RemoveAllListeners();
            UIComponent.partyButton.onClick.RemoveAllListeners();
            UIComponent.backButton.onClick.RemoveAllListeners();
            
            base.Deactivate(handle);
        }
    }
}