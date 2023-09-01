using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QuietOffliner.Core.Services
{
    public static class HexExtractService
    {
        public static IEnumerable<string?> ExtractHex(this string raw)
        {
            var regex = new Regex("([0-9a-fA-F]{2})+");
            var match = regex.Matches(raw);

            foreach (Match m in match)
                yield return m.Value;
        }

        public static string HexToString(this IEnumerable<string?> hex, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var hexArr = hex as string?[] ?? hex.ToArray();
            
            byte[] res = new byte[hexArr.Length];
            for (var i = 0; i < hexArr.Length; i++)
                res[i] = Convert.ToByte(hexArr[i], 16);

            return encoding.GetString(res);
        }
    }
}