namespace Sample.Lifecycle {
    /// <summary>
    /// BattleHud用のScreenNode
    /// </summary>
    public sealed class BattleHudScreenNode : ScreenNode<BattleHudUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "BattleHud";
    }
}