namespace Sample.Presentation {
    /// <summary>
    /// BattleHudUI用のPresenter
    /// </summary>
    public sealed class BattleHudScreenPresenter : ScreenPresenter {
        public BattleHudUI uiComponent;

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        private void OnEnable() {
            uiComponent.fullScreenButton.onClick.AddListener(() => {
                AppNavigator.GoToBattlePause();
            });
            uiComponent.backButton.onClick.AddListener(() => {
                AppNavigator.Back();
            });
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        private void OnDisable() {
            uiComponent.fullScreenButton.onClick.RemoveAllListeners();
            uiComponent.backButton.onClick.RemoveAllListeners();
        }
    }
}