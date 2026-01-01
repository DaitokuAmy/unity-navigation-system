using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityNavigationSystem {
    /// <summary>
    /// NavNode構造を管理するツリー
    /// </summary>
    public sealed class NavNodeTree : ITransitionResolver {
        /// <summary>
        /// 遷移情報
        /// </summary>
        private class TransitionInfo : ITransitionInfo<INavNode> {
            public IReadOnlyList<INavNode> PrevNodes { get; set; }
            public IReadOnlyList<INavNode> NextNodes { get; set; }
            public IReadOnlyList<ITransitionEffect> Effects { get; set; }
            public bool EffectActive { get; set; }

            public TransitionDirection Direction { get; set; }
            public TransitionState State { get; set; }
            public INavNode Prev => PrevNodes.FirstOrDefault();
            public INavNode Next => NextNodes.LastOrDefault();

            /// <inheritdoc/>
            public event Action<INavNode> FinishedEvent;

            public void SendFinish() {
                FinishedEvent?.Invoke(Next);
                FinishedEvent = null;
            }
        }

        /// <summary>
        /// プリロード情報
        /// </summary>
        private class PreLoadInfo {
            public INavNode Node;
            public PreLoadState State;
            public AsyncOperator AsyncOperator;
        }

        private readonly RootNode _rootNode;
        private readonly Dictionary<Type, INavNode> _nodeMap = new();
        private readonly List<INavNode> _runningNodes = new();
        private readonly Dictionary<Type, PreLoadInfo> _preLoadInfos = new();

        private INavNode _currentNode;
        private TransitionInfo _transitionInfo;
        private CoroutineRunner _coroutineRunner;

        /// <summary>遷移中か</summary>
        public bool IsTransitioning => _transitionInfo != null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rootNode">ルートとして登録するノード</param>
        public NavNodeTree(RootNode rootNode) {
            _rootNode = rootNode;
        }

        /// <inheritdoc/>
        void ITransitionResolver.Start() {
            _transitionInfo.State = TransitionState.Standby;
            foreach (var effect in _transitionInfo.Effects) {
                effect.BeginTransition();
            }

            // 遷移開始時に全部のフォーカスを外す
            foreach (var node in _transitionInfo.PrevNodes) {
                node.SetFocus(false);
            }

            foreach (var node in _runningNodes) {
                node.SetFocus(false);
            }
        }

        /// <inheritdoc/>
        IEnumerator ITransitionResolver.EnterEffectRoutine() {
            yield return new MergedCoroutine(_transitionInfo.Effects.Select(x => x.EnterEffectRoutine()).ToArray());
            _transitionInfo.EffectActive = true;
        }

        /// <inheritdoc/>
        IEnumerator ITransitionResolver.ExitEffectRoutine() {
            _transitionInfo.EffectActive = false;
            yield return new MergedCoroutine(_transitionInfo.Effects.Select(x => x.ExitEffectRoutine()).ToArray());
        }

        /// <inheritdoc/>
        IEnumerator ITransitionResolver.LoadNextRoutine() {
            _transitionInfo.State = TransitionState.Initializing;

            var handle = new TransitionHandle<INavNode>(_transitionInfo);
            var routines = new List<IEnumerator>();
            for (var i = 0; i < _transitionInfo.NextNodes.Count; i++) {
                routines.Add(_transitionInfo.NextNodes[i].LoadRoutine(handle));
            }

            yield return new MergedCoroutine(routines);

            for (var i = 0; i < _transitionInfo.NextNodes.Count; i++) {
                yield return _transitionInfo.NextNodes[i].InitializeRoutine(handle);
            }
        }

        /// <inheritdoc/>
        void ITransitionResolver.ActivateNext() {
            var handle = new TransitionHandle<INavNode>(_transitionInfo);
            for (var i = 0; i < _transitionInfo.NextNodes.Count; i++) {
                _transitionInfo.NextNodes[i].Activate(handle);
            }
        }

        /// <inheritdoc/>
        IEnumerator ITransitionResolver.OpenNextRoutine(bool immediate) {
            _transitionInfo.State = TransitionState.Opening;

            var handle = new TransitionHandle<INavNode>(_transitionInfo);
            for (var i = 0; i < _transitionInfo.NextNodes.Count; i++) {
                if (_transitionInfo.NextNodes[i] is IScreenNode screenNode) {
                    screenNode.PreOpen(handle);
                }
            }

            if (!immediate) {
                var routines = new List<IEnumerator>();
                for (var i = 0; i < _transitionInfo.NextNodes.Count; i++) {
                    if (_transitionInfo.NextNodes[i] is IScreenNode screenNode) {
                        routines.Add(screenNode.OpenRoutine(handle));
                    }
                }

                yield return new MergedCoroutine(routines);
            }

            for (var i = 0; i < _transitionInfo.NextNodes.Count; i++) {
                if (_transitionInfo.NextNodes[i] is IScreenNode screenNode) {
                    screenNode.PostOpen(handle);
                }
            }
        }

        /// <inheritdoc/>
        IEnumerator ITransitionResolver.ClosePrevRoutine(bool immediate) {
            var handle = new TransitionHandle<INavNode>(_transitionInfo);
            for (var i = 0; i < _transitionInfo.PrevNodes.Count; i++) {
                if (_transitionInfo.PrevNodes[i] is IScreenNode screenNode) {
                    screenNode.PreClose(handle);
                }
            }

            if (!immediate) {
                var routines = new List<IEnumerator>();
                for (var i = 0; i < _transitionInfo.PrevNodes.Count; i++) {
                    if (_transitionInfo.PrevNodes[i] is IScreenNode screenNode) {
                        routines.Add(screenNode.CloseRoutine(handle));
                    }
                }

                yield return new MergedCoroutine(routines);
            }

            for (var i = 0; i < _transitionInfo.PrevNodes.Count; i++) {
                if (_transitionInfo.PrevNodes[i] is IScreenNode screenNode) {
                    screenNode.PostClose(handle);
                }
            }
        }

        /// <inheritdoc/>
        void ITransitionResolver.DeactivatePrev() {
            var handle = new TransitionHandle<INavNode>(_transitionInfo);
            for (var i = 0; i < _transitionInfo.PrevNodes.Count; i++) {
                _transitionInfo.PrevNodes[i].Deactivate(handle);
            }
        }

        /// <inheritdoc/>
        void ITransitionResolver.UnloadPrev() {
            var handle = new TransitionHandle<INavNode>(_transitionInfo);
            for (var i = 0; i < _transitionInfo.PrevNodes.Count; i++) {
                _transitionInfo.PrevNodes[i].Terminate(handle);
            }

            for (var i = 0; i < _transitionInfo.PrevNodes.Count; i++) {
                _transitionInfo.PrevNodes[i].Unload(handle);
            }
        }

        /// <inheritdoc/>
        void ITransitionResolver.Finish() {
            foreach (var effect in _transitionInfo.Effects) {
                effect.EndTransition();
            }

            // 遷移完了時に先頭にいる物にフォーカスを充てる
            if (_runningNodes.Count > 0) {
                _runningNodes[^1].SetFocus(true);
            }

            _transitionInfo.State = TransitionState.Completed;
        }

        /// <summary>
        /// 遷移処理
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="back"></param>
        /// <param name="setupAction"></param>
        /// <param name="transition"></param>
        /// <param name="effects"></param>
        public TransitionHandle<INavNode> TransitionTo(Type nodeType, bool back, Action<INavNode> setupAction, ITransition transition, params ITransitionEffect[] effects) {
            if (IsTransitioning) {
                throw new Exception("In transitioning");
            }

            if (!_nodeMap.TryGetValue(nodeType, out var nextNode)) {
                throw new KeyNotFoundException($"Not found nodeType:{nodeType.Name}");
            }

            // 遷移する必要がなければ無視
            if (_currentNode == nextNode) {
                return TransitionHandle<INavNode>.Empty;
            }

            var prevNode = _currentNode;

            // 遷移先の共通親を探す
            var baseParent = prevNode;
            while (baseParent != null) {
                var p = nextNode;
                while (p != null) {
                    if (p == baseParent) {
                        break;
                    }

                    p = p.Parent;
                }

                if (p != null) {
                    break;
                }

                baseParent = baseParent.Parent;
            }

            // 閉じるNodeリスト
            var prevNodes = new List<INavNode>();
            if (prevNode != null) {
                var p = prevNode;
                while (p != baseParent) {
                    prevNodes.Add(p);
                    p = p.Parent;
                }
            }

            // 開くNodeリスト
            var nextNodes = new List<INavNode>();
            if (nextNode != null) {
                var p = nextNode;
                while (p != baseParent) {
                    nextNodes.Insert(0, p);
                    p = p.Parent;
                }
            }

            // 遷移情報を生成            
            _transitionInfo = new TransitionInfo {
                Direction = back ? TransitionDirection.Back : TransitionDirection.Forward,
                PrevNodes = prevNodes,
                NextNodes = nextNodes,
                State = TransitionState.Standby,
                Effects = effects
            };

            // コルーチンの登録
            void FinishTransition() {
                var transitionInfo = _transitionInfo;
                _transitionInfo = null;
                transitionInfo.SendFinish();
            }

            _coroutineRunner.StartCoroutine(TransitionRoutine(nextNode, setupAction, transition),
                FinishTransition,
                FinishTransition,
                ex => {
                    Debug.LogException(ex);
                    FinishTransition();
                });

            // ハンドルの返却
            return new TransitionHandle<INavNode>(_transitionInfo);
        }

        /// <summary>
        /// NavNodeのプリロード
        /// </summary>
        public AsyncOperationHandle PreLoadAsync(Type nodeType) {
            if (!_nodeMap.TryGetValue(nodeType, out var node)) {
                return AsyncOperationHandle.CanceledHandle;
            }

            // PreLoad情報を確認
            if (!_preLoadInfos.TryGetValue(nodeType, out var preLoadInfo)) {
                preLoadInfo = new PreLoadInfo { Node = node, State = PreLoadState.None, AsyncOperator = new AsyncOperator() };
                _preLoadInfos.Add(nodeType, preLoadInfo);
            }

            // PreLoad実行済み
            if (preLoadInfo.State != PreLoadState.None) {
                return preLoadInfo.AsyncOperator;
            }

            // Load実行
            _coroutineRunner.StartCoroutine(PreLoadRoutine(preLoadInfo),
                () => { preLoadInfo.AsyncOperator.Completed(); }, 
                () => { preLoadInfo.AsyncOperator.Aborted(); },
                ex => { preLoadInfo.AsyncOperator.Aborted(ex); });
            return preLoadInfo.AsyncOperator;
        }

        /// <summary>
        /// NavNodeのプリロード状態解除
        /// </summary>
        public void UnPreLoad(Type nodeType) {
            if (!_nodeMap.TryGetValue(nodeType, out var node)) {
                return;
            }

            // PreLoad情報から取り出し
            if (!_preLoadInfos.Remove(nodeType, out var preLoadInfo)) {
                return;
            }

            // Unload実行
            preLoadInfo.State = PreLoadState.None;
            if (!preLoadInfo.AsyncOperator.IsDone) {
                preLoadInfo.AsyncOperator.Completed();
            }

            preLoadInfo.Node.Unload(TransitionHandle<INavNode>.Empty);
        }

        /// <summary>
        /// 遷移ルーチン
        /// </summary>
        private IEnumerator TransitionRoutine(INavNode nextNode, Action<INavNode> setupAction, ITransition transition) {
            // アクティブなNodeの更新
            _runningNodes.Clear();
            {
                var n = nextNode;
                while (n != null) {
                    _runningNodes.Insert(0, n);
                    n = n.Parent;
                }
            }

            // 初期化処理
            setupAction?.Invoke(_currentNode);

            yield return transition.TransitionRoutine(this);
        }

        /// <summary>
        /// プリロードコルーチン
        /// </summary>
        private IEnumerator PreLoadRoutine(PreLoadInfo preLoadInfo) {
            if (preLoadInfo.State == PreLoadState.PreLoaded) {
                yield break;
            }

            preLoadInfo.State = PreLoadState.PreLoading;
            yield return preLoadInfo.Node.LoadRoutine(TransitionHandle<INavNode>.Empty);
            preLoadInfo.State = PreLoadState.PreLoaded;
        }
    }
}