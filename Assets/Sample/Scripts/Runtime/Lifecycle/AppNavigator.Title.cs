using Sample.Application;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator : IAppNavigator {
        /// <inheritdoc/>
        void IAppNavigator.GoToTitle() {
            _engine.TransitionTo<TitleTopScreenNode>(null, OutInTransition);
        }
    }
}