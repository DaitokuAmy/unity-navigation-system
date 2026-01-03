using System;
using UnityEngine;
using UnityNavigationSystem;
using VContainer;

namespace Sample {
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
        private void Start() {
            var containerBuilder = new ContainerBuilder();
            _rootResolver = containerBuilder.Build();

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
                                battleHud.AddScreen(new BattleDialogScreenNode());
                            });
                        });
                })
                .CreateRouter(tree => {
                    var router = new NavNodeTreeRouter(tree);
                    StateTreeRouterBuilder<Type, INavNode, NavNodeTree.TransitionOption>.Create()
                        .AddRoot(typeof(TitleTopScreenNode), titleTop => {
                            titleTop.Connect(typeof(HomeTopScreenNode), homeTop => {
                                homeTop.SetFallback(homeTop);
                                homeTop.Connect(typeof(GachaTopScreenNode), gachaTop => {
                                        gachaTop.SetFallback(homeTop);
                                    })
                                    .Connect(typeof(PartyTopScreenNode), partyTop => {
                                        partyTop.SetFallback(homeTop);
                                    })
                                    .Connect(typeof(BattleHudScreenNode), battleHud => {
                                        battleHud.Connect(typeof(BattleDialogScreenNode))
                                            .SetFallback();
                                    });
                            });
                        })
                        .Build(router);
                    return router;
                })
                .Build(_rootResolver);

            _navigationEngine.TransitionTo<TitleTopScreenNode>(null, new OutInTransition());
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