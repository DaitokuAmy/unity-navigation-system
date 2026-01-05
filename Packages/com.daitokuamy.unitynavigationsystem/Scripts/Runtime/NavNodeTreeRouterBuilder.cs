using System;
using System.Collections.Generic;

namespace UnityNavigationSystem {
    /// <summary>
    /// StateTreeRouter用のBuilder
    /// </summary>
    public sealed class NavNodeTreeNodeBuilder {
        /// <summary>
        /// Fallback情報
        /// </summary>
        private class FallbackInfo {
            public Type BaseNodeKey;
            public NavNodeTreeNodeBuilder BaseNodeBuilder;
        }

        private readonly NavNodeTreeNodeBuilder _parent;
        private readonly Type _key;
        private readonly List<NavNodeTreeNodeBuilder> _children = new();
        private readonly List<FallbackInfo> _fallbackInfos = new();

        private StateTreeNode<Type> _node;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal NavNodeTreeNodeBuilder(Type key, NavNodeTreeNodeBuilder parent, Action<NavNodeTreeNodeBuilder> buildAction) {
            _key = key;
            _parent = parent;
            buildAction?.Invoke(this);
        }

        /// <summary>
        /// 遷移先の接続
        /// </summary>
        /// <param name="key">接続先Nodeを表すキー</param>
        /// <param name="buildAction">ネスト時に利用するアクション</param>
        public NavNodeTreeNodeBuilder Connect(Type key, Action<NavNodeTreeNodeBuilder> buildAction = null) {
            var child = new NavNodeTreeNodeBuilder(key, this, buildAction);
            _children.Add(child);
            return this;
        }

        /// <summary>
        /// Fallbackの指定
        /// </summary>
        /// <param name="baseNodeKey">スコープを表すNodeKey(nullならグローバルスコープ)</param>
        public NavNodeTreeNodeBuilder SetFallback(Type baseNodeKey = null) {
            _fallbackInfos.Add(new FallbackInfo { BaseNodeKey = baseNodeKey });
            return this;
        }

        /// <summary>
        /// Fallbackの指定
        /// </summary>
        /// <param name="baseNodeBuilder">スコープを表すNodeBuilder</param>
        public NavNodeTreeNodeBuilder SetFallback(NavNodeTreeNodeBuilder baseNodeBuilder) {
            _fallbackInfos.Add(new FallbackInfo { BaseNodeBuilder = baseNodeBuilder });
            return this;
        }

        /// <summary>
        /// 構築処理
        /// </summary>
        internal void Build(NavNodeTreeRouter router) {
            _node = router.ConnectRoot(_key);

            foreach (var fallbackInfo in _fallbackInfos) {
                var baseNode = fallbackInfo.BaseNodeBuilder?._node ?? FindNodeInParent(fallbackInfo.BaseNodeKey);
                router.SetFallbackNode(_node, baseNode);
            }

            foreach (var child in _children) {
                child.Build(router, _node);
            }
        }

        /// <summary>
        /// 構築処理
        /// </summary>
        internal void Build(NavNodeTreeRouter router, StateTreeNode<Type> parent) {
            _node = parent.Connect(_key);

            foreach (var fallbackInfo in _fallbackInfos) {
                var baseNode = fallbackInfo.BaseNodeBuilder?._node ?? FindNodeInParent(fallbackInfo.BaseNodeKey);
                router.SetFallbackNode(_node, baseNode);
            }

            foreach (var child in _children) {
                child.Build(router, _node);
            }
        }

        /// <summary>
        /// 親要素の生成済みNodeを再帰的に探す
        /// </summary>
        private StateTreeNode<Type> FindNodeInParent(Type key) {
            if (key == null) {
                return null;
            }

            var p = _parent;
            while (p != null) {
                if (p._key == key) {
                    return p._node;
                }

                p = p._parent;
            }

            throw new KeyNotFoundException($"Not found base node key:{key}");
        }
    }

    /// <summary>
    /// StateTreeRouterを構築するためのBuilder
    /// </summary>
    public sealed class NavNodeTreeRouterBuilder {
        private readonly List<NavNodeTreeNodeBuilder> _rootBuilders = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private NavNodeTreeRouterBuilder() {
        }

        /// <summary>
        /// Builderの生成
        /// </summary>
        public static NavNodeTreeRouterBuilder Create() {
            return new NavNodeTreeRouterBuilder();
        }

        /// <summary>
        /// ルートの追加
        /// </summary>
        public NavNodeTreeRouterBuilder AddRoot(Type key, Action<NavNodeTreeNodeBuilder> buildAction = null) {
            var builder = new NavNodeTreeNodeBuilder(key, null, buildAction);
            _rootBuilders.Add(builder);
            return this;
        }

        /// <summary>
        /// 構築処理
        /// </summary>
        public NavNodeTreeRouter Build(NavNodeTreeRouter router) {
            foreach (var rootBuilder in _rootBuilders) {
                rootBuilder.Build(router);
            }

            return router;
        }
    }
}