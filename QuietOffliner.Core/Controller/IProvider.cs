using System.Collections.Immutable;
using System.Threading.Tasks;
using QuietOffliner.Core.Model;
using QuietOffliner.Core.Model.SearchInfos;
using QuietOffliner.Core.Model.Web;

namespace QuietOffliner.Core.Controller
{
    public interface IProvider
    {
        public string Name { get; }
        public string HomePageUri { get; }

        public Task<Request<Episode?>> LoadEpisode(string id);
        public Task<Request<ImmutableArray<EpisodeInfo>>> LoadEpisodeInfos(string query);
        public Task<Request<ImmutableArray<EpisodeInfo>>> LoadRecentEpisodeInfos();
        
        public Task<Request<ImmutableArray<SeriesInfo>>> LoadRecentSeriesInfos();
        public Task<Request<ImmutableArray<SeriesInfo>>> LoadAllSeriesInfos(SearchRequest request);
    }
}