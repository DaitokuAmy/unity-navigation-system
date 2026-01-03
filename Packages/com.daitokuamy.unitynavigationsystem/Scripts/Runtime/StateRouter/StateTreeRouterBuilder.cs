using System;
using System.Collections.Generic;

namespace UnityNavigationSystem {
    /// <summary>
    /// StateTreeRouter用のBuilder
    /// </summary>
    public sealed class StateTreeNodeBuilder<TKey, TState, TOption>
        where TKey : class
        where TState : class {
        /// <summary>
        /// Fallback情報
        /// </summary>
        private class FallbackInfo {
            public TKey BaseNodeKey;
            public StateTreeNodeBuilder<TKey, TState, TOption> BaseNodeBuilder;
        }

        private readonly StateTreeNodeBuilder<TKey, TState, TOption> _parent;
        private readonly TKey _key;
        private readonly List<StateTreeNodeBuilder<TKey, TState, TOption>> _children = new();
        private readonly List<FallbackInfo> _fallbackInfos = new();

        private StateTreeNode<TKey> _node;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal StateTreeNodeBuilder(TKey key, StateTreeNodeBuilder<TKey, TState, TOption> parent, Action<StateTreeNodeBuilder<TKey, TState, TOption>> buildAction) {
            _key = key;
            _parent = parent;
            buildAction?.Invoke(this);
        }

        /// <summary>
        /// 遷移先の接続
        /// </summary>
        /// <param name="key">接続先Nodeを表すキー</param>
        /// <param name="buildAction">ネスト時に利用するアクション</param>
        public StateTreeNodeBuilder<TKey, TState, TOption> Connect(TKey key, Action<StateTreeNodeBuilder<TKey, TState, TOption>> buildAction = null) {
            var child = new StateTreeNodeBuilder<TKey, TState, TOption>(key, this, buildAction);
            _children.Add(child);
            return this;
        }

        /// <summary>
        /// Fallbackの指定
        /// </summary>
        /// <param name="baseNodeKey">スコープを表すNodeKey(nullならグローバルスコープ)</param>
        public StateTreeNodeBuilder<TKey, TState, TOption> SetFallback(TKey baseNodeKey = null) {
            _fallbackInfos.Add(new FallbackInfo { BaseNodeKey = baseNodeKey });
            return this;
        }

        /// <summary>
        /// Fallbackの指定
        /// </summary>
        /// <param name="baseNodeBuilder">スコープを表すNodeBuilder</param>
        public StateTreeNodeBuilder<TKey, TState, TOption> SetFallback(StateTreeNodeBuilder<TKey, TState, TOption> baseNodeBuilder) {
            _fallbackInfos.Add(new FallbackInfo { BaseNodeBuilder = baseNodeBuilder });
            return this;
        }

        /// <summary>
        /// 構築処理
        /// </summary>
        internal void Build(StateTreeRouter<TKey, TState, TOption> router) {
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
        internal void Build(StateTreeRouter<TKey, TState, TOption> router, StateTreeNode<TKey> parent) {
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
        private StateTreeNode<TKey> FindNodeInParent(TKey key) {
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
    public sealed class StateTreeRouterBuilder<TKey, TState, TOption>
        where TKey : class
        where TState : class {
        private readonly List<StateTreeNodeBuilder<TKey, TState, TOption>> _rootBuilders = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private StateTreeRouterBuilder() {
        }

        /// <summary>
        /// Builderの生成
        /// </summary>
        public static StateTreeRouterBuilder<TKey, TState, TOption> Create() {
            return new StateTreeRouterBuilder<TKey, TState, TOption>();
        }

        /// <summary>
        /// ルートの追加
        /// </summary>
        public StateTreeRouterBuilder<TKey, TState, TOption> AddRoot(TKey key, Action<StateTreeNodeBuilder<TKey, TState, TOption>> buildAction = null) {
            var builder = new StateTreeNodeBuilder<TKey, TState, TOption>(key, null, buildAction);
            _rootBuilders.Add(builder);
            return this;
        }

        /// <summary>
        /// 構築処理
        /// </summary>
        public StateTreeRouter<TKey, TState, TOption> Build(StateTreeRouter<TKey, TState, TOption> router) {
            foreach (var rootBuilder in _rootBuilders) {
                rootBuilder.Build(router);
            }

            return router;
        }
    }
}