using System.Collections;
using Sample.Application;
using UnityNavigationSystem;
using VContainer;

namespace Sample.Lifecycle {
    /// <summary>
    /// バトル用SessionNode
    /// </summary>
    public sealed class BattleSessionNode : SceneSessionNode {
        [Inject]
        private IBattleSessionInputPort _sessionInputPort;

        /// <inheritdoc/>
        protected override string ScenePath => "battle";

        /// <inheritdoc/>
        protected override void Configure(IContainerBuilder builder) {
            base.Configure(builder);

            builder.Register<IBattleSessionInputPort, BattleSession>(Lifetime.Singleton);
        }

        /// <inheritdoc/>
        protected override IEnumerator InitializeRoutine(TransitionHandle<INavNode> handle, IScope scope) {
            yield return base.InitializeRoutine(handle, scope);
            
            // セッションの開始
            _sessionInputPort.Start();
        }

        /// <inheritdoc/>
        protected override void Terminate(TransitionHandle<INavNode> handle) {
            // セッションの終了
            _sessionInputPort.Exit();
            
            base.Terminate(handle);
        }
    }
}