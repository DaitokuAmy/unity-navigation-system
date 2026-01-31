using Sample.Application;

namespace Sample.Lifecycle {
    /// <summary>
    /// アプリ内遷移用クラス
    /// </summary>
    partial class AppNavigator {
        /// <inheritdoc/>
        void IAppNavigator.GoToHome() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            _engine.TransitionTo(Id.HomeTop, null, transition, transitionEffects);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToGacha() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            _engine.TransitionTo(Id.GachaTop, null, transition, transitionEffects);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToParty() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            _engine.TransitionTo(Id.PartyTop, null, transition, transitionEffects);
        }

        /// <inheritdoc/>
        void IAppNavigator.GoToShop() {
            var (transition, transitionEffects) = GetDefaultTransitionInfo<OutGameSessionNode>();
            if (!_engine.TryGetChildNodeId<HomeShopScreenNode>(out var nodeId)) {
                return;
            }
            
            _engine.TransitionTo(nodeId, null, transition, transitionEffects);
        }
    }
}