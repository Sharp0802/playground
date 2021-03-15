using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using QuietOffliner.Core.Controller;
using QuietOffliner.Core.Model.SearchInfos;

namespace QuietOffliner.Core.Model
{
    public readonly struct SeriesInfo
    {
        private SeriesInfo(
            string name,
            string artist,
            PublishInterval interval,
            IEnumerable<string> tags,
            IEnumerable<byte> thumbnail,
            IProvider provider)
        {
            Name = name;
            Artist = artist;
            Interval = interval;
            Tags = tags.ToImmutableArray();
            Thumbnail = thumbnail.ToImmutableArray();
            Provider = provider;
        }

        public static Task<SeriesInfo> New(
            string name,
            string artist,
            PublishInterval interval,
            IEnumerable<string> tags,
            IEnumerable<byte> thumbnail,
            IProvider provider)
        {
            return Task.FromResult(new SeriesInfo(
                name,
                artist, 
                interval,
                tags,
                thumbnail,
                provider));
        }

        public string Name { get; }

        public string Artist { get; }
        
        public PublishInterval Interval { get; }

        public ImmutableArray<string> Tags { get; }

        public ImmutableArray<byte> Thumbnail { get; }

        public IProvider Provider { get; }
    }
}