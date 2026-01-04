using Sample.UI;

namespace Sample.Lifecycle {
    /// <summary>
    /// Home共通用のScreenNode
    /// </summary>
    public sealed class HomeScreenNode : ScreenNode<HomeUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "Home";
    }
}