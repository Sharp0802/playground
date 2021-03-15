using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace QuietOffliner.Core.Services
{
    // ReSharper disable once InconsistentNaming
    public static class FileIOService
    {
        public static Task SaveAll(this IEnumerable<byte[]> data, Func<int, string> namingRule, string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir).Create();

            var idx = 0;
            foreach (var inData in data)
                File.WriteAllBytes(Path.Combine(dir, namingRule(idx++)), inData);
            
            return Task.CompletedTask;
        }
    }
}