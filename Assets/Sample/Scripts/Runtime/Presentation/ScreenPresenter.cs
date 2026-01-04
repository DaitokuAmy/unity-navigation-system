using Sample.Application;
using UnityEngine;
using VContainer;

namespace Sample.Presentation {
    /// <summary>
    /// ScreenUI用のPresenter
    /// </summary>
    public abstract class ScreenPresenter : MonoBehaviour {
        /// <summary>アプリ遷移用</summary>
        [Inject]
        protected IAppNavigator AppNavigator { get; private set; }
    }
}