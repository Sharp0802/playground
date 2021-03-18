using System;
using System.Diagnostics;
using System.Threading.Tasks;
using QuietOffliner.JMana.Controller;

namespace QuietOffliner.Cli
{
    internal static class Program
    {
        private static async Task Main()
        {
            var provider = new JManaProvider(1);

            
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
            
            /*var req = await provider.LoadSeriesInfo("허구추리");
            
            Console.WriteLine(req.ResponseCode.ToString());

            Console.WriteLine(req.Value?.Name);
            Console.WriteLine(req.Value?.Artist);
            Console.WriteLine(req.Value?.Tags.Aggregate((c, n) => c + "," + n));
            Console.WriteLine(req.Value?.Provider.Name);*/

            /*var req = await provider.LoadRecentEpisodeInfos();

            foreach (var ep in req.Value)
            {
                Console.WriteLine(ep.Name);
                Console.WriteLine(ep.Id);
                Console.WriteLine(ep.Date?.ToString("yyyy-MM-dd"));
                Console.WriteLine(ep.Provider.Name);
            }*/
            
            
            Console.WriteLine("Ends");
        }
    }
}