using R3;
using Sample.UI;
using UnityNavigationSystem;

namespace Sample.Presentation {
    /// <summary>
    /// TitleTopUI用のPresenter
    /// </summary>
    public sealed class TitleTopScreenPresenter : ScreenPresenter {
        public TitleTopUI uiComponent;

        /// <inheritdoc/>
        protected override void Activate(IScope scope) {
            uiComponent.fullScreenButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.GoToHome();
                });
        }
    }
}