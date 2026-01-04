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
    public partial class AppNavigator : IAppNavigator {
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
        /// 戻る遷移
        /// </summary>
        void IAppNavigator.Back(int depth) {
            var (transition, effects) = GetDefaultBackTransitionInfo();
            _engine.Back(depth, null, transition, effects);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="navigationEngine">遷移に利用するEngine</param>
        public void Initialize(NavigationEngine navigationEngine) {
            _engine = navigationEngine;
        }

        /// <summary>
        /// デフォルトの遷移情報取得
        /// </summary>
        private (ITransition ITransition,ITransitionEffect[]) GetDefaultTransitionInfo<TSessionNodeType>()
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
        private (ITransition ITransition,ITransitionEffect[]) GetDefaultBackTransitionInfo() {
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