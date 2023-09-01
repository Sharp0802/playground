using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using QuietOffliner.Core.Controller;

namespace QuietOffliner.Core.Model
{
    public readonly struct Episode
    {
        private Episode(
            string id,
            string name,
            IEnumerable<byte[]> images,
            DateTime? date,
            IProvider provider)
        {
            Id = id;
            Name = name;
            Images = images.ToImmutableArray();
            Date = date;
            Provider = provider;
        }

        public static Task<Episode> New(
            string id,
            string name,
            IEnumerable<byte[]> images,
            DateTime? date,
            IProvider provider)
        {
            return Task.FromResult(new Episode(
                id,
                name,
                images,
                date,
                provider));
        }

        public string Id { get; }
        
        public string Name { get; }

        public ImmutableArray<byte[]> Images { get; }
        
        public DateTime? Date { get; }

        public IProvider Provider { get; }
    }
}