using R3;
using Sample.UI;
using UnityNavigationSystem;

namespace Sample.Presentation {
    /// <summary>
    /// BattleHudUI用のPresenter
    /// </summary>
    public sealed class BattleHudScreenPresenter : ScreenPresenter {
        public BattleHudUI uiComponent;

        /// <inheritdoc/>
        protected override void Activate(IScope scope) {
            uiComponent.fullScreenButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.GoToBattlePause();
                });
            uiComponent.backButton.OnClickAsObservable()
                .TakeUntil(scope)
                .Subscribe(_ => {
                    AppNavigator.Back();
                });
        }
    }
}