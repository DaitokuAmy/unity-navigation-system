namespace Sample {
    /// <summary>
    /// タイトル用SessionNode
    /// </summary>
    public sealed class TitleSessionNode : SceneSessionNode {
        /// <inheritdoc/>
        protected override string EmptyScenePath => "empty";
        /// <inheritdoc/>
        protected override string ScenePath => "title";
    }
}