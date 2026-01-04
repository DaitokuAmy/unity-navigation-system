using Sample.UI;

namespace Sample.Lifecycle {
    /// <summary>
    /// GachaTop用のScreenNode
    /// </summary>
    public sealed class GachaTopScreenNode : ScreenNode<GachaTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "GachaTop";
    }
}