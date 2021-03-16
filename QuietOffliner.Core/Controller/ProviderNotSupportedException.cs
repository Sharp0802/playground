using System;

namespace QuietOffliner.Core.Controller
{
    public class ProviderNotSupportedException : NotSupportedException
    {
        public ProviderNotSupportedException(IProvider provider, string cause) : base(cause)
        {
            Provider = provider;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private IProvider Provider { get; }
    }
}