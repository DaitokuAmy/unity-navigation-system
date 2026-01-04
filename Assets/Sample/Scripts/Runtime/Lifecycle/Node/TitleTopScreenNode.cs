namespace Sample.Lifecycle {
    /// <summary>
    /// TitleTop用のScreenNode
    /// </summary>
    public sealed class TitleTopScreenNode : ScreenNode<TitleTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "TitleTop";
    }
}