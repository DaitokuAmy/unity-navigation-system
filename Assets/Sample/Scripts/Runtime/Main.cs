using UnityEngine;
using UnityNavigationSystem;
using VContainer;

namespace Sample {
    /// <summary>
    /// メイン処理
    /// </summary>
    public class Main : MonoBehaviour {
        private IObjectResolver _rootResolver;
        private NavigationEngine _navigationEngine;

        /// <summary>
        /// 生成時処理
        /// </summary>
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        private void Start() {
            var builder = new ContainerBuilder();
            _rootResolver = builder.Build();

            _navigationEngine = NavigationEngineBuilder.Create()
                .CreateTree(new RootNode(), root => {
                    root.AddSession(new TitleSessionNode(), session => {
                        session.AddScreen(new TitleTopScreenNode());
                    });
                    root.AddSession(new BattleSessionNode(), session => {
                        session.AddScreen(new BattleHudScreenNode(), battleHud => {
                            battleHud.AddScreen(new BattleDialogScreenNode());
                        });
                    });
                })
                .CreateRouter(tree => {
                    var router = new NavNodeTreeRouter(tree);
                    var titleTop = router.ConnectRoot(typeof(TitleTopScreenNode));
                    var battleHud = titleTop.Connect(typeof(BattleHudScreenNode));
                    var battleDialog = battleHud.Connect(typeof(BattleDialogScreenNode));
                    router.SetFallbackNode(battleHud);
                    return router;
                })
                .Build(_rootResolver);

            _navigationEngine.TransitionTo<TitleTopScreenNode>(null, new OutInTransition());
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        private void Update() {
            _navigationEngine?.Update();
        }

        /// <summary>
        /// 廃棄時処理
        /// </summary>
        private void OnDestroy() {
            _navigationEngine?.Dispose();
        }
    }
}