namespace QuietOffliner.WebP.Enums
{
    /// <summary>Enumeration of the status codes.</summary>
    public enum Vp8StatusCode
    {
        /// <summary>No error.</summary>
        Vp8StatusOk = 0,
        /// <summary>Memory error allocating objects.</summary>
        Vp8StatusOutOfMemory,
        /// <summary>Configuration is invalid.</summary>
        Vp8StatusInvalidParam,
        Vp8StatusBitstreamError,
        /// <summary>Configuration is invalid.</summary>
        Vp8StatusUnsupportedFeature,
        Vp8StatusSuspended,
        /// <summary>Abort request by user.</summary>
        Vp8StatusUserAbort,
        Vp8StatusNotEnoughData,
    }
}