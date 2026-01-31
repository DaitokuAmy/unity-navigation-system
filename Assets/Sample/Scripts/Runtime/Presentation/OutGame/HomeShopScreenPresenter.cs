using R3;
using Sample.UI;
using UnityNavigationSystem;

namespace Sample.Presentation {
    /// <summary>
    /// HomeShopUI用のPresenter
    /// </summary>
    public sealed class HomeShopScreenPresenter : ScreenPresenter {
        public HomeShopUI uiComponent;

        /// <inheritdoc/>
        protected override void Activate(IScope scope) {
            uiComponent.closeButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.Back();
                });
        }
    }
}