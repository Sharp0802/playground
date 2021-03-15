using System;
using System.Threading.Tasks;
using QuietOffliner.Core.Controller;

namespace QuietOffliner.Core.Model
{
    public readonly struct EpisodeInfo
    {
        private EpisodeInfo(
            string id,
            string name,
            DateTime? date,
            IProvider provider)
        {
            Id = id;
            Name = name;
            Date = date;
            Provider = provider;
        }

        public static Task<EpisodeInfo> New(
            string id,
            string name,
            DateTime? date,
            IProvider provider)
        {
            return Task.FromResult(new EpisodeInfo(
                id,
                name,
                date,
                provider));
        }

        public string Id { get; }
        
        public string Name { get; }

        public DateTime? Date { get; }

        public IProvider Provider { get; }
    }
}