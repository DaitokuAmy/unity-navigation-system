using System.Collections;

namespace UnityNavigationSystem {
    /// <summary>
    /// NavNode基底
    /// </summary>
    public abstract class NavNode : INavNode {
        private INavNode _parent;

        /// <inheritdoc/>
        INavNode INavNode.Parent => _parent;

        /// <inheritdoc/>
        bool INavNode.CanAddNode(INavNode node) {
            return CanAddNode(node);
        }

        /// <inheritdoc/>
        void INavNode.SetFocus(bool focus) {
        }

        /// <inheritdoc/>
        IEnumerator INavNode.LoadRoutine(TransitionHandle<INavNode> handle) {
            yield return LoadRoutine(handle);
        }

        /// <inheritdoc/>
        IEnumerator INavNode.InitializeRoutine(TransitionHandle<INavNode> handle) {
            yield return InitializeRoutine(handle);
        }

        /// <inheritdoc/>
        void INavNode.Activate(TransitionHandle<INavNode> handle) {
            Activate(handle);
        }

        /// <inheritdoc/>
        void INavNode.Deactivate(TransitionHandle<INavNode> handle) {
            Deactivate(handle);
        }

        /// <inheritdoc/>
        void INavNode.Terminate(TransitionHandle<INavNode> handle) {
            Terminate(handle);
        }

        /// <inheritdoc/>
        void INavNode.Unload(TransitionHandle<INavNode> handle) {
            Unload(handle);
        }

        /// <summary>
        /// 該当Nodeを子として登録可能か
        /// </summary>
        /// <param name="node">子に登録する候補となるNode参照</param>
        protected abstract bool CanAddNode(INavNode node);

        /// <summary>
        /// フォーカス状態の設定
        /// </summary>
        /// <param name="focus">フォーカス状態</param>
        protected virtual void SetFocus(bool focus) { }

        /// <summary>
        /// 読み込み処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual IEnumerator LoadRoutine(TransitionHandle<INavNode> handle) {
            yield break;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual IEnumerator InitializeRoutine(TransitionHandle<INavNode> handle) {
            yield break;
        }

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void Activate(TransitionHandle<INavNode> handle) { }

        /// <summary>
        /// 非アクティブ時処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void Deactivate(TransitionHandle<INavNode> handle) { }

        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void Terminate(TransitionHandle<INavNode> handle) { }

        /// <summary>
        /// アンロード処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void Unload(TransitionHandle<INavNode> handle) { }
    }
}