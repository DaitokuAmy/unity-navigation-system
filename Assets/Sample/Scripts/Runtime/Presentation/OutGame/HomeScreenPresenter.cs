using Sample.UI;

namespace Sample.Presentation {
    /// <summary>
    /// HomeUI用のPresenter
    /// </summary>
    public sealed class HomeScreenPresenter : ScreenPresenter {
        public HomeUI uiComponent;

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        private void OnEnable() {
            uiComponent.backButton.onClick.AddListener(() => {
                AppNavigator.Back();
            });
            uiComponent.homeButton.onClick.AddListener(() => {
                AppNavigator.GoToHome();
            });
            uiComponent.gachaButton.onClick.AddListener(() => {
                AppNavigator.GoToGacha();
            });
            uiComponent.partyButton.onClick.AddListener(() => {
                AppNavigator.GoToParty();
            });
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        private void OnDisable() {
            uiComponent.backButton.onClick.RemoveAllListeners();
            uiComponent.homeButton.onClick.RemoveAllListeners();
            uiComponent.gachaButton.onClick.RemoveAllListeners();
            uiComponent.partyButton.onClick.RemoveAllListeners();
        }
    }
}