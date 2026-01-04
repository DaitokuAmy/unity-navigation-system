namespace Sample.Presentation {
    /// <summary>
    /// TitleTopUI用のPresenter
    /// </summary>
    public sealed class TitleTopScreenPresenter : ScreenPresenter {
        public TitleTopUI uiComponent;

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        private void OnEnable() {
            uiComponent.fullScreenButton.onClick.AddListener(() => {
                AppNavigator.GoToHome();
            });
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        private void OnDisable() {
            uiComponent.fullScreenButton.onClick.RemoveAllListeners();
        }
    }
}