using Sample.Application;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator : IAppNavigator {
        /// <inheritdoc/>
        void IAppNavigator.GoToBattle() {
            _engine.TransitionTo<BattleHudScreenNode>(null, OutInTransition);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToBattlePause() {
            var transition = OutInTransition;
            if (_engine.CheckCurrentNodeParentType<BattleSessionNode>()) {
                transition = CrossTransition;
            }

            _engine.TransitionTo<BattlePauseScreenNode>(null, transition);
        }
    }
}