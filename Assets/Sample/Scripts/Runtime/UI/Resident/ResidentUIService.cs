namespace Sample.UI {
    /// <summary>
    /// 常駐UI用サービス
    /// </summary>
    public sealed class ResidentUIService {
        private ResidentUI _uiComponent;
        
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize(ResidentUI uiComponent) {
            _uiComponent = uiComponent;
        }

        /// <summary>
        /// Loadingの表示
        /// </summary>
        public void ShowLoading() {
            _uiComponent.loadingUI.Show();
        }

        /// <summary>
        /// Loadingの非表示
        /// </summary>
        public void HideLoading() {
            _uiComponent.loadingUI.Hide();
        }
    }
}