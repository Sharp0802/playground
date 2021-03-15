using System;
using System.Linq;
using System.Threading.Tasks;
using QuietOffliner.Core.Services;
using QuietOffliner.JMana.Controller;

namespace QuietOffliner.Cli
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var provider = await Provider.New();

            
            /*var req = await provider.LoadEpisode(429623.ToString());
            
            Console.WriteLine(req.ResponseCode.ToString());

            if (req.Value is not null)
                await req.Value.Value.Images.SaveAll(i => {
                    Console.WriteLine(i);
                    return $"{i.ToString()}.webp";
                }, $"./downloads/{req.Value.Value.Name} by {req.Value.Value.Provider.Name}");*/

            /*var req = await provider.LoadEpisodeInfos("허구추리");
            
            Console.WriteLine(req.ResponseCode.ToString());

            foreach (var ep in req.Value)
            {
                if (ep.Date != null)
                    Console.WriteLine($"{ep.Date.Value:yyyy-MM-dd}");
                Console.WriteLine(ep.Id);
                Console.WriteLine(ep.Name);
                Console.WriteLine(ep.Provider.Name);
            }*/

            var req = await provider.LoadRecentSeriesInfos();
            
            Console.WriteLine(req.ResponseCode.ToString());

            foreach (var series in req.Value)
            {
                Console.WriteLine(series.Name);
                Console.WriteLine(series.Artist);
                Console.WriteLine(series.Tags.Aggregate((c, n) => c + "," + n));
                Console.WriteLine(series.Provider.Name);
            }
            
            
            Console.WriteLine("Ends");
        }
    }
}