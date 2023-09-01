namespace QuietOffliner.WebP.Enums
{
    /// <summary>Encoding error conditions.</summary>
    public enum WebPEncodingError
    {
        /// <summary>No error.</summary>
        Vp8EncOk = 0,
        /// <summary>Memory error allocating objects.</summary>
        Vp8EncErrorOutOfMemory,
        /// <summary>Memory error while flushing bits.</summary>
        Vp8EncErrorBitstreamOutOfMemory,
        /// <summary>A  pointer parameter is NULL.</summary>
        Vp8EncErrorNullParameter,
        /// <summary>Configuration is invalid.</summary>
        Vp8EncErrorInvalidConfiguration,
        /// <summary>Picture has invalid width/height.</summary>
        Vp8EncErrorBadDimension,
        /// <summary>Partition is bigger than 512k.</summary>
        Vp8EncErrorPartition0Overflow,
        /// <summary>Partition is bigger than 16M.</summary>
        Vp8EncErrorPartitionOverflow,
        /// <summary>Error while flushing bytes.</summary>
        Vp8EncErrorBadWrite,
        /// <summary>File is bigger than 4G.</summary>
        Vp8EncErrorFileTooBig,
        /// <summary>Abort request by user.</summary>
        Vp8EncErrorUserAbort,
        /// <summary>List terminator. always last.</summary>
        Vp8EncErrorLast,
    }
}