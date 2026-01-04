using Sample.UI;

namespace Sample.Presentation {
    /// <summary>
    /// BattlePauseUI用のPresenter
    /// </summary>
    public sealed class BattlePauseScreenPresenter : ScreenPresenter {
        public BattlePauseUI uiComponent;

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        private void OnEnable() {
            uiComponent.cancelButton.onClick.AddListener(() => {
                AppNavigator.Back();
            });
            uiComponent.decideButton.onClick.AddListener(() => {
                AppNavigator.GoToTitle();
            });
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        private void OnDisable() {
            uiComponent.cancelButton.onClick.RemoveAllListeners();
            uiComponent.decideButton.onClick.RemoveAllListeners();
        }
    }
}