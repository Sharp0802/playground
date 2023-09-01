namespace QuietOffliner.WebP.Enums
{
    /// <summary>Enumerate some predefined settings for WebPConfig, depending on the type of source picture. These presets are used when calling WebPConfigPreset().</summary>
    public enum WebPPreset
    {
        /// <summary>Default preset.</summary>
        WebpPresetDefault = 0,
        /// <summary>Digital picture, like portrait, inner shot.</summary>
        WebpPresetPicture,
        /// <summary>Outdoor photograph, with natural lighting.</summary>
        WebpPresetPhoto,
        /// <summary>Hand or line drawing, with high-contrast details.</summary>
        WebpPresetDrawing,
        /// <summary>Small-sized colorful images.</summary>
        WebpPresetIcon,
        /// <summary>Text-like.</summary>
        WebpPresetText
    }
}