using System.Collections;

namespace UnityNavigationSystem {
    /// <summary>
    /// コルーチン用ユーティリティ
    /// </summary>
    internal static class CoroutineUtils {
        /// <summary>
        /// コルーチンの並列結合
        /// </summary>
        public static IEnumerator Merge(params IEnumerator[] enumerators) {
            yield return new MergedCoroutine(enumerators);
        }
    }
}