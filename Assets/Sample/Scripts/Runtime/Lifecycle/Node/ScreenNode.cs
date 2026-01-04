using System.Collections;
using Sample.Application;
using Sample.Presentation;
using UnityEngine;
using UnityNavigationSystem;
using VContainer;

namespace Sample.Lifecycle {
    /// <summary>
    /// ScreenNode基底
    /// </summary>
    public abstract class ScreenNode<TUIComponent> : ScreenNode
        where TUIComponent : MonoBehaviour {
        private GameObject _loadedPrefab;
        private GameObject _createdObject;

        /// <summary>Resources内のPrefab配置Path</summary>
        protected abstract string PrefabPath { get; }

        /// <summary>アプリ遷移用インスタンス</summary>
        [Inject]
        protected IAppNavigator AppNavigator { get; private set; }
        /// <summary>生成したUIComponent</summary>
        protected TUIComponent UIComponent { get; private set; }

        /// <inheritdoc/>
        protected override IEnumerator LoadRoutine(TransitionHandle<INavNode> handle, IScope scope) {
            if (!string.IsNullOrEmpty(PrefabPath)) {
                var request = Resources.LoadAsync<GameObject>(PrefabPath);
                yield return request;
                _loadedPrefab = request.asset as GameObject;
            }
        }

        /// <inheritdoc/>
        protected override IEnumerator InitializeRoutine(TransitionHandle<INavNode> handle, IScope scope) {
            if (_loadedPrefab != null) {
                _createdObject = Object.Instantiate(_loadedPrefab);
                UIComponent = _createdObject.GetComponent<TUIComponent>();
                var presenter = _createdObject.GetComponent<ScreenPresenter>();
                if (presenter != null) {
                    ObjectResolver.Inject(presenter);
                }
            }

            yield break;
        }

        /// <inheritdoc/>
        protected override void Terminate(TransitionHandle<INavNode> handle) {
            if (UIComponent != null) {
                Object.Destroy(_createdObject);
                UIComponent = null;
                _createdObject = null;
            }
        }

        /// <inheritdoc/>
        protected override void Unload(TransitionHandle<INavNode> handle) {
            _loadedPrefab = null;
        }
    }
}