using Sample.UI;

namespace Sample.Lifecycle {
    /// <summary>
    /// HomeTop用のScreenNode
    /// </summary>
    public sealed class HomeTopScreenNode : ScreenNode<HomeTopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "HomeTop";
    }
}