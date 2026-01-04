using Sample.Application;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator {
        /// <inheritdoc/>
        void IAppNavigator.GoToBattle() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<BattleSessionNode>();
            _engine.TransitionTo<BattleHudScreenNode>(null, transition, transitionEffects);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToBattlePause() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<BattleSessionNode>();
            _engine.TransitionTo<BattlePauseScreenNode>(null, transition, transitionEffects);
        }
    }
}