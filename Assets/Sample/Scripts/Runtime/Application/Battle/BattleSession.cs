using System;
using UnityEngine;

namespace Sample.Application {
    /// <summary>
    /// バトルセッション用インターフェース
    /// </summary>
    public class BattleSession : IBattleSessionInputPort, IDisposable {
        /// <summary>
        /// 廃棄処理
        /// </summary>
        public void Dispose() {
        }
        
        /// <summary>
        /// 開始
        /// </summary>
        void IBattleSessionInputPort.Start() {
            Debug.Log("Start Battle");
        }

        /// <summary>
        /// 終了
        /// </summary>
        void IBattleSessionInputPort.Exit() {
            Debug.Log("Exit Battle");
        }
    }
}