namespace QuietOffliner.Core.Controller
{
	public struct ProviderSupportList
	{
		public bool CanLogin;
		
		public bool EpisodeInfosPageIndexing;
		public bool RecentEpisodeInfosPageIndexing;
		
		public bool CanLoadSeriesInfo;
		public bool SeriesInfosPageIndexing;
		public bool RecentSeriesInfosPageIndexing;

		public ProviderSupportList(
			bool canLogin,
			
			bool episodeInfosPageIndexing,
			bool recentEpisodeInfosPageIndexing,
			
			bool canLoadSeriesInfo,
			bool seriesInfosPageIndexing,
			bool recentSeriesInfosPageIndexing)
		{
			CanLogin = canLogin;
			EpisodeInfosPageIndexing = episodeInfosPageIndexing;
			RecentEpisodeInfosPageIndexing = recentEpisodeInfosPageIndexing;
			CanLoadSeriesInfo = canLoadSeriesInfo;
			SeriesInfosPageIndexing = seriesInfosPageIndexing;
			RecentSeriesInfosPageIndexing = recentSeriesInfosPageIndexing;
		}
	}
}