using UnityEngine;

namespace Sample.UI {
    /// <summary>
    /// ローディングUI
    /// </summary>
    public sealed class LoadingUI : MonoBehaviour {
        private static int OnShowHash => Animator.StringToHash("OnShow");
        
        [SerializeField]
        private Animator _animator;
        
        /// <summary>
        /// 表示
        /// </summary>
        public void Show() {
            gameObject.SetActive(true);
            _animator.SetTrigger(OnShowHash);
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public void Hide() {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 生成時処理
        /// </summary>
        private void Awake() {
            Hide();
        }
    }
}