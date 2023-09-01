namespace QuietOffliner.Core.Controller
{
	public readonly struct ProviderSupportList
	{
		public bool CanLogin { get; }

		public bool EpisodeInfosPageIndexing { get; }
		public bool RecentEpisodeInfosPageIndexing { get; }
		
		public bool CanLoadSeriesInfo { get; }
		public bool SeriesInfosPageIndexing { get; }
		public bool RecentSeriesInfosPageIndexing { get; }

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