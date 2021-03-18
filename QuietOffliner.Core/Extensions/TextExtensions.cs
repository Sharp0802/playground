using System.Text;

namespace QuietOffliner.Core.Extensions
{
	public static class TextExtensions
	{
		public static string ToString(this byte[] bytes, Encoding? encoding)
		{
			encoding ??= Encoding.UTF8;

			return encoding.GetString(bytes);
		}

		public static Encoding GetEncoding(this string encoding)
		{
			try
			{
				return Encoding.GetEncoding(encoding);
			}
			catch
			{
				return Encoding.UTF8;
			}
		}
	}
}