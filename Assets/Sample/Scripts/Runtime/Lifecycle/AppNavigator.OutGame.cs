using Sample.Application;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator : IAppNavigator {
        /// <inheritdoc/>
        void IAppNavigator.GoToHome() {
            var transition = OutInTransition;
            if (_engine.CheckCurrentNodeParentType<OutGameSessionNode>()) {
                transition = CrossTransition;
            }

            _engine.TransitionTo<HomeTopScreenNode>(null, transition);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToGacha() {
            var transition = OutInTransition;
            if (_engine.CheckCurrentNodeParentType<OutGameSessionNode>()) {
                transition = CrossTransition;
            }

            _engine.TransitionTo<GachaTopScreenNode>(null, transition);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToParty() {
            var transition = OutInTransition;
            if (_engine.CheckCurrentNodeParentType<OutGameSessionNode>()) {
                transition = CrossTransition;
            }

            _engine.TransitionTo<PartyTopScreenNode>(null, transition);
        }
    }
}