using System;
using System.Collections.Generic;

namespace UnityNavigationSystem {
    /// <summary>
    /// Navigationシステムを動かすためのエンジン
    /// </summary>
    public sealed class NavigationEngine : IDisposable {
        private readonly List<INavNode> _nodes = new();
        private readonly NavNodeTree _tree;
        private readonly IStateRouter<Type, INavNode, NavNodeTree.TransitionOption> _router;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal NavigationEngine(IRootNode rootNode, IReadOnlyDictionary<Type, INavNode> nodeMap, Func<NavNodeTree, IStateRouter<Type, INavNode, NavNodeTree.TransitionOption>> createRouterFunc) {
            _nodes.AddRange(nodeMap.Values);
            _tree = new NavNodeTree(rootNode, nodeMap, this);
            _router = createRouterFunc.Invoke(_tree);
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
        /// <param name="option">遷移オプション</param>
        /// <param name="setupAction">遷移先Node</param>
        /// <param name="transition">遷移方法</param>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> TransitionTo<TNode>(NavNodeTree.TransitionOption option, Action<TNode> setupAction, ITransition transition, params ITransitionEffect[] effects)
            where TNode : IScreenNode {
            if (_router != null) {
                return _router.TransitionTo(typeof(TNode), option, node => {
                    setupAction?.Invoke((TNode)node);
                }, transition, effects);
            }

            return _tree.TransitionTo(typeof(TNode), option, false, node => {
                setupAction?.Invoke((TNode)node);
            }, transition, effects);
        }

        /// <summary>
        /// 遷移実行
        /// </summary>
        /// <param name="setupAction">遷移先Node</param>
        /// <param name="transition">遷移方法</param>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> TransitionTo<TNode>(Action<TNode> setupAction, ITransition transition, params ITransitionEffect[] effects)
            where TNode : IScreenNode {
            return TransitionTo(null, setupAction, transition, effects);
        }

        /// <summary>
        /// 遷移実行
        /// </summary>
        /// <param name="transition">遷移方法</param>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> TransitionTo<TNode>(ITransition transition, params ITransitionEffect[] effects)
            where TNode : IScreenNode {
            return TransitionTo<TNode>(null, null, transition, effects);
        }

        /// <summary>
        /// 戻る処理
        /// </summary>
        /// <param name="depth">戻り階層数(1～)</param>
        /// <param name="option">遷移時に渡すオプション</param>
        /// <param name="setupAction">遷移先初期化用関数</param>
        /// <param name="transition">遷移方法</param>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> Back(int depth = 1, NavNodeTree.TransitionOption option = null, Action<INavNode> setupAction = null, ITransition transition = null, params ITransitionEffect[] effects) {
            if (_router != null) {
                return _router.Back(depth, option, setupAction, transition, effects);
            }
            
            throw new NotSupportedException("null router is not supported.");
        }

        /// <summary>
        /// 戻る処理
        /// </summary>
        /// <param name="setupAction">遷移先初期化用関数</param>
        /// <param name="transition">遷移方法</param>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> Back(Action<INavNode> setupAction, ITransition transition = null, params ITransitionEffect[] effects) {
            return Back(1, null, setupAction, transition, effects);
        }

        /// <summary>
        /// 戻る処理
        /// </summary>
        /// <param name="transition">遷移方法</param>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> Back(ITransition transition = null, params ITransitionEffect[] effects) {
            return Back(1, null, null, transition, effects);
        }
        
        /// <summary>
        /// 状態リセット
        /// </summary>
        /// <param name="setupAction">遷移先初期化用関数</param>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> Reset(Action<INavNode> setupAction, params ITransitionEffect[] effects) {
            if (_router != null) {
                return _router.Reset(setupAction, effects);
            }

            return _tree.Reset(setupAction, effects);
        }
        
        /// <summary>
        /// 状態リセット
        /// </summary>
        /// <param name="effects">遷移時演出</param>
        public TransitionHandle<INavNode> Reset(params ITransitionEffect[] effects) {
            return Reset(null, effects);
        }
    }
}