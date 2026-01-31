using Sample.UI;

namespace Sample.Lifecycle {
    /// <summary>
    /// Home用ショップモーダルのScreenNode
    /// </summary>
    public sealed class HomeShopScreenNode : ScreenNode<HomeShopUI> {
        /// <inheritdoc/>
        protected override string PrefabPath => "HomeShop";
    }
}