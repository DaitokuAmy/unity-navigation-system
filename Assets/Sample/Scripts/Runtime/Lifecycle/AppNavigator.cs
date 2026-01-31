using System;
using System.Collections;
using Sample.Application;
using Sample.UI;
using UnityEngine;
using UnityNavigationSystem;
using VContainer;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    public partial class AppNavigator : IAppNavigator, IDisposable {
        /// <summary>
        /// ローディング画面用の遷移演出
        /// </summary>
        private class LoadingEffect : ITransitionEffect {
            private readonly ResidentUIService _residentUIService;
            private readonly float _minShowTime;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public LoadingEffect(ResidentUIService residentUIService, float minShowTime = 0.0f) {
                _residentUIService = residentUIService;
                _minShowTime = minShowTime;
            }

            /// <inheritdoc/>
            void ITransitionEffect.BeginTransition() { }

            /// <inheritdoc/>
            IEnumerator ITransitionEffect.EnterEffectRoutine() {
                _residentUIService.ShowLoading();
                yield return new WaitForSeconds(_minShowTime);
            }

            /// <inheritdoc/>
            void ITransitionEffect.Update() { }

            /// <inheritdoc/>
            IEnumerator ITransitionEffect.ExitEffectRoutine() {
                _residentUIService.HideLoading();
                yield break;
            }

            /// <inheritdoc/>
            void ITransitionEffect.EndTransition() { }
        }

        /// <summary>OutIn遷移</summary>
        private static readonly ITransition OutInTransition = new OutInTransition();
        /// <summary>Cross遷移</summary>
        private static readonly ITransition CrossTransition = new CrossTransition();

        private NavigationEngine _engine;

        private ITransitionEffect[] _loadingEffects = Array.Empty<ITransitionEffect>();

        /// <summary>
        /// Inject処理
        /// </summary>
        [Inject]
        public void Construct(ResidentUIService residentUIService) {
            _loadingEffects = new ITransitionEffect[] { new LoadingEffect(residentUIService, 2.0f) };
        }

        /// <summary>
        /// 廃棄時処理
        /// </summary>
        public void Dispose() {
            _engine?.Dispose();
            _engine = null;
        }

        /// <summary>
        /// 戻る遷移
        /// </summary>
        void IAppNavigator.Back(int depth) {
            var (transition, effects) = GetDefaultBackTransitionInfo();
            _engine.Back(depth, null, transition, effects);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="rootResolver">親要素に入れたいDIContainerのResolver</param>
        public void Initialize(IObjectResolver rootResolver = null) {
            _engine = NavigationEngineBuilder.Create()
                .CreateLifecycle<RootNode>(Id.Root, root => {
                    root.AddSession<TitleSessionNode>(Id.Title, title => {
                            title.AddScreen<TitleTopScreenNode>(Id.TitleTop);
                        })
                        .AddSession<OutGameSessionNode>(Id.OutGame, outGame => {
                            outGame.AddScreen<HomeScreenNode>(Id.Home, home => {
                                home.AddScreen<HomeTopScreenNode>(Id.HomeTop, homeTop => {
                                        homeTop.AddScreen<HomeShopScreenNode>(Id.HomeShopOnTop);
                                    })
                                    .AddScreen<GachaTopScreenNode>(Id.GachaTop, gachaTop => {
                                        gachaTop.AddScreen<HomeShopScreenNode>(Id.HomeShopOnGacha);
                                    })
                                    .AddScreen<PartyTopScreenNode>(Id.PartyTop, homeTop => {
                                        homeTop.AddScreen<HomeShopScreenNode>(Id.HomeShopOnParty);
                                    });
                            });
                        })
                        .AddSession<BattleSessionNode>(Id.Battle, battle => {
                            battle.AddScreen<BattleHudScreenNode>(Id.BattleHud, battleHud => {
                                battleHud.AddScreen<BattlePauseScreenNode>(Id.BattlePause);
                            });
                        });
                })
                .CreateRouter(container => {
                    var router = new NavNodeTreeRouter(container);
                    NavNodeTreeRouterBuilder.Create()
                        .AddRoot(Id.TitleTop, titleTop => {
                            titleTop.Connect(Id.HomeTop, homeTop => {
                                homeTop.SetShortcutScope(homeTop)
                                    .Connect(Id.HomeShopOnTop)
                                    .Connect(Id.GachaTop, gachaTop => {
                                        gachaTop.SetShortcutScope(homeTop)
                                            .Connect(Id.HomeShopOnGacha);
                                    })
                                    .Connect(Id.PartyTop, partyTop => {
                                        partyTop.SetShortcutScope(homeTop)
                                            .Connect(Id.HomeShopOnParty);
                                    })
                                    .Connect(Id.BattleHud, battleHud => {
                                        battleHud.SetGlobalShortcut()
                                            .Connect(Id.BattlePause);
                                    });
                            });
                        })
                        .Build(router);
                    return router;
                })
                .Build(rootResolver);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update() {
            _engine?.Update();
        }

        /// <summary>
        /// デフォルトの遷移情報取得
        /// </summary>
        private (ITransition ITransition, ITransitionEffect[]) GetDefaultTransitionInfo<TSessionNodeType>()
            where TSessionNodeType : SceneSessionNode {
            var transition = CrossTransition;
            var effects = Array.Empty<ITransitionEffect>();

            // 行き先のSessionNodeを含んでいるか
            var sessionNode = _engine.GetNodeInParent<TSessionNodeType>();
            if (sessionNode == null) {
                // OutInTransition, LoadingEffectsにする
                transition = OutInTransition;
                effects = _loadingEffects;
            }

            return (transition, effects);
        }

        /// <summary>
        /// デフォルトの戻り遷移情報取得
        /// </summary>
        private (ITransition ITransition, ITransitionEffect[]) GetDefaultBackTransitionInfo() {
            var transition = CrossTransition;
            var effects = Array.Empty<ITransitionEffect>();

            // 現在のSessionNodeと戻り先のSessionNodeを比較
            var currentSessionNode = _engine.GetNodeInParent<SceneSessionNode>();
            var backSessionNode = _engine.GetBackNodeInParent<SceneSessionNode>();
            if (currentSessionNode != backSessionNode) {
                // OutInTransition, LoadingEffectsにする
                transition = OutInTransition;
                effects = _loadingEffects;
            }

            return (transition, effects);
        }
    }
}