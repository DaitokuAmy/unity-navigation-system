using R3;
using Sample.UI;
using UnityNavigationSystem;

namespace Sample.Presentation {
    /// <summary>
    /// HomeUI用のPresenter
    /// </summary>
    public sealed class HomeScreenPresenter : ScreenPresenter {
        public HomeUI uiComponent;

        /// <inheritdoc/>
        protected override void Activate(IScope scope) {
            uiComponent.backButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.Back();
                });
            uiComponent.homeButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.GoToHome();
                });
            uiComponent.gachaButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.GoToGacha();
                });
            uiComponent.partyButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.GoToParty();
                });
        }
    }
}