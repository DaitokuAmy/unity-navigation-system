using Sample.Application;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator {
        /// <inheritdoc/>
        void IAppNavigator.GoToTitle() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            _engine.TransitionTo(Id.TitleTop, null, transition, transitionEffects);
        }
    }
}