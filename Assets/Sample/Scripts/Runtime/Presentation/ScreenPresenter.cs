using Sample.Application;
using UnityEngine;
using UnityNavigationSystem;
using VContainer;

namespace Sample.Presentation {
    /// <summary>
    /// ScreenUI用のPresenter
    /// </summary>
    public abstract class ScreenPresenter : MonoBehaviour {
        private DisposableScope _scope;
        
        /// <summary>アプリ遷移用</summary>
        [Inject]
        protected IAppNavigator AppNavigator { get; private set; }

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        private void OnEnable() {
            _scope = new DisposableScope();
            Activate(_scope);
        }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        private void OnDisable() {
            Deactivate();
            _scope.Dispose();
            _scope = null;
        }
        
        /// <summary>
        /// アクティブ時処理
        /// </summary>
        /// <param name="scope">生存Scope</param>
        protected virtual void Activate(IScope scope) {}
        
        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        protected virtual void Deactivate() {}
    }
}