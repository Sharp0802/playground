using System;

namespace QuietOffliner.Core.Controller
{
    public class ProviderNotSupportedException : NotSupportedException
    {
        public ProviderNotSupportedException(IProvider provider, string cause) : base(cause)
        {
            Provider = provider;
        }

        private IProvider Provider { get; }
    }
}