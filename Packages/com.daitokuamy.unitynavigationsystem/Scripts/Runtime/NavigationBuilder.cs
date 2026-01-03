using System;
using System.Collections.Generic;
using VContainer;

namespace UnityNavigationSystem {
    using NavNodeRouter = IStateRouter<Type, INavNode, NavNodeTree.TransitionOption>;
    
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
        void Build(INavNode parentNode, Dictionary<Type, INavNode> nodeMap);
    }

    /// <summary>
    /// RootNode用のBuilder
    /// </summary>
    public sealed class RootNodeBuilder {
        private readonly IRootNode _rootNode;
        private readonly List<INavNodeBuilder> _childBuilders = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal RootNodeBuilder(IRootNode rootNode, Action<RootNodeBuilder> buildAction = null) {
            _rootNode = rootNode;
            buildAction?.Invoke(this);
        }

        /// <summary>
        /// SessionNodeの追加
        /// </summary>
        /// <param name="sessionNode">追加対象のNode</param>
        /// <param name="buildAction">子要素を追加するためのアクション</param>
        public RootNodeBuilder AddSession(ISessionNode sessionNode, Action<SessionNodeBuilder> buildAction = null) {
            var child = new SessionNodeBuilder(sessionNode, buildAction);
            _childBuilders.Add(child);
            return this;
        }

        /// <summary>
        /// ビルド処理
        /// </summary>
        /// <param name="nodeMap">KeyValue登録用の辞書</param>
        /// <param name="parentObjectResolver">親として設定するVContainerのResolver</param>
        internal IRootNode Build(Dictionary<Type, INavNode> nodeMap, IObjectResolver parentObjectResolver) {
            if (!nodeMap.TryAdd(_rootNode.GetType(), _rootNode)) {
                throw new InvalidOperationException($"Node type {_rootNode.GetType()} is already registered.");
            }

            _rootNode.SetParent(null, parentObjectResolver);
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
        private readonly ISessionNode _sessionNode;
        private readonly List<INavNodeBuilder> _childBuilders = new();

        /// <inheritdoc/>
        INavNode INavNodeBuilder.Node => _sessionNode;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal SessionNodeBuilder(ISessionNode sessionNode, Action<SessionNodeBuilder> buildAction = null) {
            _sessionNode = sessionNode;
            buildAction?.Invoke(this);
        }

        /// <inheritdoc/>
        void INavNodeBuilder.Build(INavNode parentNode, Dictionary<Type, INavNode> nodeMap) {
            if (!nodeMap.TryAdd(_sessionNode.GetType(), _sessionNode)) {
                throw new InvalidOperationException($"Node type {_sessionNode.GetType()} is already registered.");
            }

            _sessionNode.SetParent(parentNode, parentNode.ObjectResolver);
            foreach (var child in _childBuilders) {
                child.Build(_sessionNode, nodeMap);
            }
        }

        /// <summary>
        /// ScreenNodeの追加
        /// </summary>
        /// <param name="screenNode">追加対象のNode</param>
        /// <param name="buildAction">子要素を追加するためのアクション</param>
        public SessionNodeBuilder AddScreen(IScreenNode screenNode, Action<ScreenNodeBuilder> buildAction = null) {
            var child = new ScreenNodeBuilder(screenNode, buildAction);
            _childBuilders.Add(child);
            return this;
        }
    }

    /// <summary>
    /// ScreenNode用のBuilder
    /// </summary>
    public sealed class ScreenNodeBuilder : INavNodeBuilder {
        private readonly IScreenNode _screenNode;
        private readonly List<INavNodeBuilder> _childBuilders = new();

        /// <inheritdoc/>
        INavNode INavNodeBuilder.Node => _screenNode;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal ScreenNodeBuilder(IScreenNode screenNode, Action<ScreenNodeBuilder> buildAction = null) {
            _screenNode = screenNode;
            buildAction?.Invoke(this);
        }

        /// <inheritdoc/>
        void INavNodeBuilder.Build(INavNode parentNode, Dictionary<Type, INavNode> nodeMap) {
            if (!nodeMap.TryAdd(_screenNode.GetType(), _screenNode)) {
                throw new InvalidOperationException($"Node type {_screenNode.GetType()} is already registered.");
            }

            _screenNode.SetParent(parentNode, parentNode.ObjectResolver);
            foreach (var child in _childBuilders) {
                child.Build(_screenNode, nodeMap);
            }
        }

        /// <summary>
        /// ScreenNodeの追加
        /// </summary>
        /// <param name="screenNode">追加対象のNode</param>
        /// <param name="buildAction">子要素を追加するためのアクション</param>
        public ScreenNodeBuilder AddScreen(IScreenNode screenNode, Action<ScreenNodeBuilder> buildAction = null) {
            var child = new ScreenNodeBuilder(screenNode, buildAction);
            _childBuilders.Add(child);
            return this;
        }
    }

    /// <summary>
    /// NavigationEngineのBuilder
    /// </summary>
    public sealed class NavigationEngineBuilder {
        
        private RootNodeBuilder _rootNodeBuilder;
        private Func<NavNodeTree, NavNodeRouter> _createRouterFunc;
        private Dictionary<Type, INavNode> _nodeMap = new();

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
        /// Nodeツリーの生成
        /// </summary>
        /// <param name="rootNode">登録するRootNode</param>
        /// <param name="buildAction">子要素を登録するためのアクション</param>
        public NavigationEngineBuilder CreateTree(IRootNode rootNode, Action<RootNodeBuilder> buildAction) {
            if (_rootNodeBuilder != null) {
                throw new InvalidOperationException("RootNode is already set.");
            }

            _rootNodeBuilder = new RootNodeBuilder(rootNode, buildAction);
            return this;
        }

        /// <summary>
        /// Node遷移用のルーター設定
        /// </summary>
        /// <param name="createFunc">Routerの生成処理</param>
        public NavigationEngineBuilder CreateRouter(Func<NavNodeTree, NavNodeRouter> createFunc) {
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