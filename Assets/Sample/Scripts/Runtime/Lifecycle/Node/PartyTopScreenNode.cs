namespace Sample.Lifecycle {
    /// <summary>
    /// PartyTop用のScreenNode
    /// </summary>
    public sealed class PartyTopScreenNode : ScreenNode<PartyTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "PartyTop";
    }
}