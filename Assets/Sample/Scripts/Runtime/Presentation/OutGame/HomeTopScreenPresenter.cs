namespace Sample.Presentation {
    /// <summary>
    /// HomeTopUI用のPresenter
    /// </summary>
    public sealed class HomeTopScreenPresenter : ScreenPresenter {
        public HomeTopUI uiComponent;

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        private void OnEnable() {
            uiComponent.battleButton.onClick.AddListener(() => {
                AppNavigator.GoToBattle();
            });
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        private void OnDisable() {
            uiComponent.battleButton.onClick.RemoveAllListeners();
        }
    }
}