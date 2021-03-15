using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using QuietOffliner.Core.Controller;
using QuietOffliner.Core.Model.SearchInfos;

namespace QuietOffliner.Core.Model
{
    public readonly struct Series
    {
        private Series(
            string name,
            string artist,
            PublishInterval interval,
            IEnumerable<string> tags,
            IEnumerable<EpisodeInfo> episodes,
            IEnumerable<byte> thumbnail,
            IProvider provider)
        {
            Name = name;
            Artist = artist;
            Interval = interval;
            Tags = tags.ToImmutableArray();
            Episodes = episodes.ToImmutableArray();
            Thumbnail = thumbnail.ToImmutableArray();
            Provider = provider;
        }

        public static Task<Series> New(
            string name,
            string artist,
            PublishInterval interval,
            IEnumerable<string> tags,
            IEnumerable<EpisodeInfo> episodes,
            IEnumerable<byte> thumbnail,
            IProvider provider)
        {
            return Task.FromResult(new Series(
                name,
                artist,
                interval,
                tags,
                episodes,
                thumbnail,
                provider));
        }

        public string Name { get; }

        public string Artist { get; }
        
        public PublishInterval Interval { get; }
        
        public ImmutableArray<string> Tags { get; }
        
        public ImmutableArray<EpisodeInfo> Episodes { get; }

        public ImmutableArray<byte> Thumbnail { get; }
        
        public IProvider Provider { get; }
    }
}