using Sample.Application;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator {
        /// <inheritdoc/>
        void IAppNavigator.GoToHome() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            _engine.TransitionTo<HomeTopScreenNode>(null, transition, transitionEffects);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToGacha() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            _engine.TransitionTo<GachaTopScreenNode>(null, transition, transitionEffects);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToParty() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            _engine.TransitionTo<PartyTopScreenNode>(null, transition, transitionEffects);
        }
    }
}