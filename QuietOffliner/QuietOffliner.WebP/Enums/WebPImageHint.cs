namespace QuietOffliner.WebP.Enums
{
    /// <summary>Image characteristics hint for the underlying encoder.</summary>
    public enum WebPImageHint
    {
        /// <summary>Default preset.</summary>
        WebpHintDefault = 0,
        /// <summary>Digital picture, like portrait, inner shot</summary>
        WebpHintPicture,
        /// <summary>Outdoor photograph, with natural lighting</summary>
        WebpHintPhoto,
        /// <summary>Discrete tone image (graph, map-tile etc).</summary>
        WebpHintGraph,
        /// <summary>list terminator. always last.</summary>
        WebpHintLast
    }
}