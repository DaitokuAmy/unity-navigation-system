using R3;
using Sample.UI;
using UnityNavigationSystem;

namespace Sample.Presentation {
    /// <summary>
    /// HomeTopUI用のPresenter
    /// </summary>
    public sealed class HomeTopScreenPresenter : ScreenPresenter {
        public HomeTopUI uiComponent;

        /// <inheritdoc/>
        protected override void Activate(IScope scope) {
            uiComponent.battleButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.GoToBattle();
                });
        }
    }
}