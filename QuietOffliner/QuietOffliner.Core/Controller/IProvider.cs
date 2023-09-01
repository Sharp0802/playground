using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using QuietOffliner.Core.Controller.Web;
using QuietOffliner.Core.Model;
using QuietOffliner.Core.Model.SearchInfos;
using QuietOffliner.Core.Model.Web;

namespace QuietOffliner.Core.Controller
{
    public interface IProvider
    {
        public string Name { get; }
        public string HomePageUri { get; }
        public ProviderSupportList SupportList { get; }

        public Task<Request<Episode?>> LoadEpisode(string id);
        public Task<Request<ImmutableArray<EpisodeInfo>>> LoadEpisodeInfos(string query, int page = 0);
        public Task<Request<ImmutableArray<EpisodeInfo>>> LoadRecentEpisodeInfos(int page = 0);
        
        public Task<Request<SeriesInfo?>> LoadSeriesInfo(string query);
        public Task<Request<ImmutableArray<SeriesInfo>>> LoadRecentSeriesInfos(int page = 0);
        public Task<Request<ImmutableArray<SeriesInfo>>> LoadAllSeriesInfos(SearchRequest request, int page = 0);
    }
    
    public abstract class Provider : IProvider, IDisposable
    {
        protected Provider(
            string name,
            string homePage,
            ProviderSupportList supportList,
            TextWriter? logStream = null)
        {
            Name = name;
            HomePageUri = homePage;
            SupportList = supportList;
            
            Client = new WebRequestClient(logStream);
        }

        public string Name { get; }
        public string HomePageUri { get; }
        public ProviderSupportList SupportList { get; }
        protected WebRequestClient Client { get; }

        public abstract Task<Request<Episode?>> LoadEpisode(string id);
        public abstract Task<Request<ImmutableArray<EpisodeInfo>>> LoadEpisodeInfos(string query, int page = 0);
        public abstract Task<Request<ImmutableArray<EpisodeInfo>>> LoadRecentEpisodeInfos(int page = 0);
        public abstract Task<Request<SeriesInfo?>> LoadSeriesInfo(string query);
        public abstract Task<Request<ImmutableArray<SeriesInfo>>> LoadRecentSeriesInfos(int page = 0);
        public abstract Task<Request<ImmutableArray<SeriesInfo>>> LoadAllSeriesInfos(SearchRequest request, int page = 0);

        ~Provider()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            Client.Dispose();
            
            GC.SuppressFinalize(this);
        }
    }
}