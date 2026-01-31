using System.Collections;
using Sample.Application;
using Sample.UI;
using UnityEngine;
using UnityNavigationSystem;
using VContainer;

namespace Sample.Lifecycle {
    /// <summary>
    /// メイン処理
    /// </summary>
    public class Main : MonoBehaviour {
        private IObjectResolver _rootResolver;
        private AppNavigator _appNavigator;

        /// <summary>
        /// 生成時処理
        /// </summary>
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        private IEnumerator Start() {
            // DIのRootContainer構築
            var containerBuilder = new ContainerBuilder();
            _appNavigator = new AppNavigator();
            containerBuilder.RegisterInstance<IAppNavigator>(_appNavigator);
            containerBuilder.Register<ResidentUIService>(Lifetime.Singleton);
            _rootResolver = containerBuilder.Build();

            // Inject
            _rootResolver.Inject(_appNavigator);

            // Navigator初期化
            _appNavigator.Initialize(_rootResolver);

            // 常駐UI生成
            var request = Resources.LoadAsync<GameObject>("Resident");
            yield return request;
            var residentPrefab = request.asset as GameObject;
            var residentObject = Instantiate(residentPrefab, transform, false);
            var residentUIService = _rootResolver.Resolve<ResidentUIService>();
            residentUIService.Initialize(residentObject.GetComponent<ResidentUI>());

            // 初期画面遷移
            var navigator = _rootResolver.Resolve<IAppNavigator>();
            navigator.GoToTitle();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void Update() {
            _appNavigator?.Update();
        }

        /// <summary>
        /// 廃棄時処理
        /// </summary>
        private void OnDestroy() {
            _appNavigator?.Dispose();
            _rootResolver?.Dispose();
        }
    }
}