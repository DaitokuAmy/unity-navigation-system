using R3;
using Sample.UI;
using UnityNavigationSystem;

namespace Sample.Presentation {
    /// <summary>
    /// BattlePauseUI用のPresenter
    /// </summary>
    public sealed class BattlePauseScreenPresenter : ScreenPresenter {
        public BattlePauseUI uiComponent;

        /// <inheritdoc/>
        protected override void Activate(IScope scope) {
            uiComponent.cancelButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.Back();
                });
            uiComponent.decideButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.GoToTitle();
                });
        }
    }
}