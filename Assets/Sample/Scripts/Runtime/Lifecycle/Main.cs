using System;
using System.Collections;
using System.Threading.Tasks;
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
        private NavigationEngine _navigationEngine;

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
            var appNavigator = new AppNavigator();
            containerBuilder.RegisterInstance<IAppNavigator>(appNavigator);
            containerBuilder.Register<ResidentUIService>(Lifetime.Singleton);
            _rootResolver = containerBuilder.Build();

            // Inject
            _rootResolver.Inject(appNavigator);

            // 遷移エンジン構築
            _navigationEngine = NavigationEngineBuilder.Create()
                .CreateTree(new RootNode(), root => {
                    root.AddSession(new TitleSessionNode(), title => {
                            title.AddScreen(new TitleTopScreenNode());
                        })
                        .AddSession(new OutGameSessionNode(), outGame => {
                            outGame.AddScreen(new HomeScreenNode(), home => {
                                home.AddScreen(new HomeTopScreenNode())
                                    .AddScreen(new GachaTopScreenNode())
                                    .AddScreen(new PartyTopScreenNode());
                            });
                        })
                        .AddSession(new BattleSessionNode(), battle => {
                            battle.AddScreen(new BattleHudScreenNode(), battleHud => {
                                battleHud.AddScreen(new BattlePauseScreenNode());
                            });
                        });
                })
                .CreateRouter(tree => {
                    var router = new NavNodeTreeRouter(tree);
                    NavNodeTreeRouterBuilder.Create()
                        .AddRoot(typeof(TitleTopScreenNode), titleTop => {
                            titleTop.Connect(typeof(HomeTopScreenNode), homeTop => {
                                homeTop.SetShortcutScope(homeTop);
                                homeTop.Connect(typeof(GachaTopScreenNode), gachaTop => {
                                        gachaTop.SetShortcutScope(homeTop);
                                    })
                                    .Connect(typeof(PartyTopScreenNode), partyTop => {
                                        partyTop.SetShortcutScope(homeTop);
                                    })
                                    .Connect(typeof(BattleHudScreenNode), battleHud => {
                                        battleHud.Connect(typeof(BattlePauseScreenNode))
                                            .SetGlobalShortcut();
                                    });
                            });
                        })
                        .Build(router);
                    return router;
                })
                .Build(_rootResolver);

            // Navigator初期化
            appNavigator.Initialize(_navigationEngine);

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
            _navigationEngine?.Update();
        }

        /// <summary>
        /// 廃棄時処理
        /// </summary>
        private void OnDestroy() {
            _navigationEngine?.Dispose();
            _rootResolver?.Dispose();
        }
    }
}