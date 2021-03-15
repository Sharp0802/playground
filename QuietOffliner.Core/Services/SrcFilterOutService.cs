using System.Collections.Generic;
using System.Linq;

namespace QuietOffliner.Core.Services
{
	public static class SrcFilterOutService
	{
		public static IEnumerable<string> FilterOutByText(
			this IEnumerable<string> src,
			string mustContains)
		{
			return src.Where(s => s.Contains(mustContains));
		}
	}
}