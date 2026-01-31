using System;
using System.Collections.Generic;
using VContainer;

namespace UnityNavigationSystem {
    /// <summary>
    /// Node構築用のインターフェース
    /// </summary>
    internal interface INavNodeBuilder {
        /// <summary>保持しているNavNode</summary>
        INavNode Node { get; }

        /// <summary>
        /// ビルド処理
        /// </summary>
        /// <param name="parentNode">登録親のNode</param>
        /// <param name="nodeMap">KeyValue登録用の辞書</param>
        void Build(INavNode parentNode, Dictionary<int, INavNode> nodeMap);
    }

    /// <summary>
    /// RootNode用のBuilder
    /// </summary>
    public sealed class RootNodeBuilder {
        private readonly int _nodeId;
        private readonly IRootNode _rootNode;
        private readonly List<INavNodeBuilder> _childBuilders = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal RootNodeBuilder(int nodeId, IRootNode rootNode, Action<RootNodeBuilder> buildAction = null) {
            _nodeId = nodeId;
            _rootNode = rootNode;
            buildAction?.Invoke(this);
        }

        /// <summary>
        /// SessionNodeの追加
        /// </summary>
        /// <param name="nodeId">登録するNode識別用Id</param>
        /// <param name="buildAction">子要素を追加するためのアクション</param>
        public RootNodeBuilder AddSession<TNode>(int nodeId, Action<SessionNodeBuilder> buildAction = null)
            where TNode : ISessionNode, new() {
            var child = new SessionNodeBuilder(nodeId, new TNode(), buildAction);
            _childBuilders.Add(child);
            return this;
        }

        /// <summary>
        /// ビルド処理
        /// </summary>
        /// <param name="nodeMap">KeyValue登録用の辞書</param>
        /// <param name="parentObjectResolver">親として設定するVContainerのResolver</param>
        internal IRootNode Build(Dictionary<int, INavNode> nodeMap, IObjectResolver parentObjectResolver) {
            if (!nodeMap.TryAdd(_nodeId, _rootNode)) {
                throw new InvalidOperationException($"Node id <{_nodeId}:{_rootNode.GetType()}> is already registered.");
            }

            _rootNode.Setup(_nodeId, null, parentObjectResolver);
            foreach (var child in _childBuilders) {
                child.Build(_rootNode, nodeMap);
            }

            return _rootNode;
        }
    }

    /// <summary>
    /// SessionNode用のBuilder
    /// </summary>
    public sealed class SessionNodeBuilder : INavNodeBuilder {
        private readonly int _nodeId;
        private readonly ISessionNode _sessionNode;
        private readonly List<INavNodeBuilder> _childBuilders = new();

        /// <inheritdoc/>
        INavNode INavNodeBuilder.Node => _sessionNode;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SessionNodeBuilder(int nodeId, ISessionNode sessionNode, Action<SessionNodeBuilder> buildAction = null) {
            _nodeId = nodeId;
            _sessionNode = sessionNode;
            buildAction?.Invoke(this);
        }

        /// <inheritdoc/>
        void INavNodeBuilder.Build(INavNode parentNode, Dictionary<int, INavNode> nodeMap) {
            if (!nodeMap.TryAdd(_nodeId, _sessionNode)) {
                throw new InvalidOperationException($"Node id <{_nodeId}:{_sessionNode.GetType()}> is already registered.");
            }

            _sessionNode.Setup(_nodeId, parentNode, parentNode.ObjectResolver);
            foreach (var child in _childBuilders) {
                child.Build(_sessionNode, nodeMap);
            }
        }

        /// <summary>
        /// ScreenNodeの追加
        /// </summary>
        /// <param name="nodeId">登録するNode識別用Id</param>
        /// <param name="buildAction">子要素を追加するためのアクション</param>
        public SessionNodeBuilder AddScreen<TNode>(int nodeId, Action<ScreenNodeBuilder> buildAction = null)
            where TNode : IScreenNode, new() {
            var child = new ScreenNodeBuilder(nodeId, new TNode(), buildAction);
            _childBuilders.Add(child);
            return this;
        }
    }

    /// <summary>
    /// ScreenNode用のBuilder
    /// </summary>
    public sealed class ScreenNodeBuilder : INavNodeBuilder {
        private readonly int _nodeId;
        private readonly IScreenNode _screenNode;
        private readonly List<INavNodeBuilder> _childBuilders = new();

        /// <inheritdoc/>
        INavNode INavNodeBuilder.Node => _screenNode;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ScreenNodeBuilder(int nodeId, IScreenNode screenNode, Action<ScreenNodeBuilder> buildAction = null) {
            _nodeId = nodeId;
            _screenNode = screenNode;
            buildAction?.Invoke(this);
        }

        /// <inheritdoc/>
        void INavNodeBuilder.Build(INavNode parentNode, Dictionary<int, INavNode> nodeMap) {
            if (!nodeMap.TryAdd(_nodeId, _screenNode)) {
                throw new InvalidOperationException($"Node id <{_nodeId}:{_screenNode.GetType()}> is already registered.");
            }

            _screenNode.Setup(_nodeId, parentNode, parentNode.ObjectResolver);
            foreach (var child in _childBuilders) {
                child.Build(_screenNode, nodeMap);
            }
        }

        /// <summary>
        /// ScreenNodeの追加
        /// </summary>
        /// <param name="nodeId">登録するNode識別用Id</param>
        /// <param name="buildAction">子要素を追加するためのアクション</param>
        public ScreenNodeBuilder AddScreen<TNode>(int nodeId, Action<ScreenNodeBuilder> buildAction = null)
            where TNode : IScreenNode, new() {
            var child = new ScreenNodeBuilder(nodeId, new TNode(), buildAction);
            _childBuilders.Add(child);
            return this;
        }
    }

    /// <summary>
    /// NavigationEngineのBuilder
    /// </summary>
    public sealed class NavigationEngineBuilder {
        private RootNodeBuilder _rootNodeBuilder;
        private Func<NavNodeTree, INavNodeStateRouter> _createRouterFunc;
        private Dictionary<int, INavNode> _nodeMap = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private NavigationEngineBuilder() {
        }

        /// <summary>
        /// ビルダーの生成
        /// </summary>
        public static NavigationEngineBuilder Create() {
            return new NavigationEngineBuilder();
        }

        /// <summary>
        /// ライフサイクルを表すツリー構造の生成
        /// </summary>
        /// <param name="nodeId">登録するNode識別用Id</param>
        /// <param name="buildAction">子要素を登録するためのアクション</param>
        public NavigationEngineBuilder CreateLifecycle<TNode>(int nodeId, Action<RootNodeBuilder> buildAction)
            where TNode : IRootNode, new() {
            if (_rootNodeBuilder != null) {
                throw new InvalidOperationException("RootNode is already set.");
            }

            _rootNodeBuilder = new RootNodeBuilder(nodeId, new TNode(), buildAction);
            return this;
        }

        /// <summary>
        /// Node遷移用のルーター設定
        /// </summary>
        /// <param name="createFunc">Routerの生成処理</param>
        public NavigationEngineBuilder CreateRouter(Func<NavNodeTree, INavNodeStateRouter> createFunc) {
            _createRouterFunc = createFunc;
            return this;
        }

        /// <summary>
        /// エンジンのビルド
        /// </summary>
        public NavigationEngine Build(IObjectResolver parentObjectResolver = null) {
            var rootNode = _rootNodeBuilder.Build(_nodeMap, parentObjectResolver);
            var engine = new NavigationEngine(rootNode, _nodeMap, _createRouterFunc);
            return engine;
        }
    }
}