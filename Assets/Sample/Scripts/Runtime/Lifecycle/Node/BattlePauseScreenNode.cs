namespace Sample.Lifecycle {
    /// <summary>
    /// BattlePause用のScreenNode
    /// </summary>
    public sealed class BattlePauseScreenNode : ScreenNode<BattlePauseUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "BattlePause";
    }
}