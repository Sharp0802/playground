using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace QuietOffliner.Core.Model.SearchInfos
{
    public readonly struct SearchRequest
    {
        private SearchRequest(
            string name,
            string artist,
            PublishInterval interval,
            Ordering ordering,
            IEnumerable<string> tags)
        {
            Name = name;
            Artist = artist;
            Interval = interval;
            Ordering = ordering;
            Tags = tags.ToImmutableArray();
        }

        public static Task<SearchRequest> New(
            string name,
            string artist,
            PublishInterval interval,
            Ordering ordering,
            IEnumerable<string> tags)
        {
            return Task.FromResult(new SearchRequest(
                name,
                artist,
                interval,
                ordering,
                tags));
        }

        public string                   Name { get; }
        public string                   Artist { get; }
        
        public PublishInterval          Interval { get; }
        public Ordering                 Ordering { get; }
        
        public ImmutableArray<string>   Tags { get; }
    }
}