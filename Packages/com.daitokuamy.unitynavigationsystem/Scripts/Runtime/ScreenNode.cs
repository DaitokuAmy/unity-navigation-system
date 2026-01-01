using System.Collections;

namespace UnityNavigationSystem {
    /// <summary>
    /// ScreenとなるNavNode
    /// ※Session or Screen直下にのみ置けるNode
    /// </summary>
    public abstract class ScreenNode : NavNode, IScreenNode {
        /// <inheritdoc/>
        void IScreenNode.PreOpen(TransitionHandle<INavNode> handle) {
            PreOpen(handle);
        }

        /// <inheritdoc/>
        IEnumerator IScreenNode.OpenRoutine(TransitionHandle<INavNode> handle) {
            yield return OpenRoutine(handle);
        }

        /// <inheritdoc/>
        void IScreenNode.PostOpen(TransitionHandle<INavNode> handle) {
            PostOpen(handle);
        }

        /// <inheritdoc/>
        void IScreenNode.PreClose(TransitionHandle<INavNode> handle) {
            PreClose(handle);
        }

        /// <inheritdoc/>
        IEnumerator IScreenNode.CloseRoutine(TransitionHandle<INavNode> handle) {
            yield return CloseRoutine(handle);
        }

        /// <inheritdoc/>
        void IScreenNode.PostClose(TransitionHandle<INavNode> handle) {
            PostClose(handle);
        }
        
        /// <inheritdoc/>
        protected sealed override bool CanAddNode(INavNode node) {
            if (node is ScreenNode) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 開く前処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void PreOpen(TransitionHandle<INavNode> handle) {
        }
        
        /// <summary>
        /// 開くアニメルーチン
        /// ※Immediate時は呼ばれない
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual IEnumerator OpenRoutine(TransitionHandle<INavNode> handle) {
            yield break;
        }

        /// <summary>
        /// 開いた後処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void PostOpen(TransitionHandle<INavNode> handle) {
        }
        
        /// <summary>
        /// 閉じる前処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void PreClose(TransitionHandle<INavNode> handle) {
        }
        
        /// <summary>
        /// 閉じるアニメルーチン
        /// ※Immediate時は呼ばれない
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual IEnumerator CloseRoutine(TransitionHandle<INavNode> handle) {
            yield break;
        }
        
        /// <summary>
        /// 閉じた後処理
        /// </summary>
        /// <param name="handle">遷移ハンドル</param>
        protected virtual void PostClose(TransitionHandle<INavNode> handle) {
        }
    }
}