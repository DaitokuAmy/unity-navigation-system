using System;
using System.Collections.Generic;
using VContainer;

namespace UnityNavigationSystem {
    /// <summary>
    /// Navigationシステムを動かすためのエンジン
    /// </summary>
    public sealed class NavigationEngine : IDisposable {
        private readonly List<INavNode> _nodes = new();
        private readonly NavNodeTree _tree;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal NavigationEngine(IRootNode rootNode, IReadOnlyDictionary<Type, INavNode> nodeMap) {
            _nodes.AddRange(nodeMap.Values);
            _tree = new NavNodeTree(rootNode, nodeMap, this);
        }

        /// <summary>
        /// 廃棄処理
        /// </summary>
        public void Dispose() {
            // Treeの廃棄
            _tree.Dispose();

            // 各種NodeのRelease
            for (var i = _nodes.Count - 1; i >= 0; i--) {
                _nodes[i].Release();
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update() {
            _tree.Update();
        }

        /// <summary>
        /// 遷移実行
        /// </summary>
        /// <param name="setupAction">遷移先Node</param>
        /// <param name="transition">遷移方法</param>
        public TransitionHandle<INavNode> TransitionTo<TNode>(Action<TNode> setupAction, ITransition transition)
            where TNode : ScreenNode {
            return _tree.TransitionTo(typeof(TNode), false, node => {
                setupAction?.Invoke((TNode)node);
            }, transition);
        }
    }
}