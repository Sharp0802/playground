using System.CommandLine;
using System.Text;

namespace BettingManager;

internal class Program
{
    private List<Person> People { get; } = new();
    private List<Station> Stations { get; } = new();

    private void Run()
    {
        var stationOpt = new Option<string>("--station", "The name of new station to create.");
        var team0Opt = new Option<string>("--team0", "The name of the team 0 of the specific station.");
        var team1Opt = new Option<string>("--team1", "The name of the team 1 of the specific station.");

        var winnerOpt = new Option<string>("--winner", "The name of winner team in the specific station.");

        var nameOpt = new Option<string>("--name", "The name of the specific end user.");
        var idOpt = new Option<ushort>("--id", "The ID of the specific end user.");
        var cntOpt = new Option<int>("--count", "The possession count");

        var teamOpt = new Option<string>("--team", "The name of target team of the specific station.");
        var betCntOpt = new Option<int>("--count", "The count to bet to the specific station.");

        var open = new Command("open", "Opens new bet-able station.");
        open.AddOption(stationOpt);
        open.AddOption(team0Opt);
        open.AddOption(team1Opt);
        open.SetHandler(OpenHandler, stationOpt, team0Opt, team1Opt);

        var close = new Command("close", "Closes the bet-able station and Rewards winnings at gambling.");
        close.AddOption(stationOpt);
        close.AddOption(winnerOpt);
        close.SetHandler(CloseHandler, stationOpt, winnerOpt);
        
        var set = new Command("set", "Set or Create a user data");
        set.AddOption(nameOpt);
        set.AddOption(idOpt);
        set.AddOption(cntOpt);
        set.SetHandler(SetHandler, nameOpt, idOpt, cntOpt);
        
        var bet = new Command("bet", "Set or create a user data");
        bet.AddOption(idOpt);
        bet.AddOption(stationOpt);
        bet.AddOption(teamOpt);
        bet.AddOption(betCntOpt);
        bet.SetHandler(BetHandler, idOpt, stationOpt, teamOpt, betCntOpt);
        
        var queryU = new Command("query-user", "Queries the user data.");
        queryU.AddOption(idOpt);
        queryU.SetHandler(UserQueryHandler, idOpt);
        
        var queryS = new Command("query-station", "Queries the station data.");
        queryS.AddOption(stationOpt);
        queryS.SetHandler(StationQueryHandler, stationOpt);

        var exit = new Command("exit", "Terminates the program.");
        exit.SetHandler(() => Environment.Exit(0));

        var root = new RootCommand("A manager application for simple betting system.");
        root.AddCommand(open);
        root.AddCommand(close);
        root.AddCommand(set);
        root.AddCommand(bet);
        root.AddCommand(queryU);
        root.AddCommand(queryS);
        root.AddCommand(exit);

        root.Invoke(Console.ReadLine() ?? "");
    }

    private void CloseHandler(string station, string winner)
    {
        var s = Stations.Find(s => s.Name.Equals(station));
        if (s is null)
        {
            Console.WriteLine("E: Failed to close station. Station data not found.");
        }
        else
        {
            if (s.Team0Alias.Equals(winner, StringComparison.Ordinal))
            {
                var total = s.Team0BetTarget.Values.Sum();
                var perArr = s.Team0BetTarget.Select(target => (target.Key, target.Value, target.Value / (double)total));
                var adder = s.Team1BetTarget.Values.Sum();
                foreach (var (person, bet, per) in perArr)
                {
                    var add = (int)Math.Round(per * adder);
                    Console.Write($"PERSON({person.Id:000000},{person.Name}) {person.PossessionCount}+{bet}+{add} -> ");
                    person.PossessionCount += bet + add;
                    Console.WriteLine($"{person.PossessionCount}");
                }
            }
            else if (s.Team1Alias.Equals(winner, StringComparison.Ordinal))
            {
                var total = s.Team1BetTarget.Values.Sum();
                var perArr = s.Team1BetTarget.Select(target => (target.Key, target.Value, target.Value / (double)total));
                var adder = s.Team0BetTarget.Values.Sum();
                foreach (var (person, bet, per) in perArr)
                {
                    var add = (int)Math.Round(per * adder);
                    Console.Write($"PERSON({person.Id:000000},{person.Name}) {person.PossessionCount}+{bet}+{add} -> ");
                    person.PossessionCount += bet + add;
                    Console.WriteLine($"{person.PossessionCount}");
                }
            }
            else
            {
                Console.WriteLine("E: Failed to close station. Team data not found.");
            }
        }
    }

    private void OpenHandler(string station, string t0, string t1)
    {
        if (Stations.Any(s => s.Name.Equals(station, StringComparison.Ordinal)))
        {
            Console.WriteLine($"E: Failed to create a new station. Already the station exist with name '{station}'.");
        }
        else if (string.IsNullOrWhiteSpace(station))
        {
            Console.WriteLine("E: Failed to create a new station. Station name cannot be null or whitespace only.");
        }
        else if (string.IsNullOrWhiteSpace(t0) || string.IsNullOrWhiteSpace(t1))
        {
            Console.WriteLine("E: Failed to create a new station. Team name cannot be null or whitespace only.");
        }
        else
        {
            Stations.Add(new Station(station, t0, t1));
            Console.WriteLine("I: Success to create a new station.");
            Console.WriteLine($"   STATION{{{station},{t0},{t1}}}");
        }
    }

    private void SetHandler(string name, ushort id, int cnt)
    {
        if (People.Any(p => p.Id == id))
        {
            Console.WriteLine($"W: Already the data exist with id '{id}'.\n" + $"Data will be overwritten and cannot be recovered.\n" + $"Force to proceed it? [y/N]");
            var ans = Console.ReadLine();
            if (ans is not null && ans.Length == 1 && (ans[0] == 'y' || ans[0] == 'Y'))
            {
                People.RemoveAll(p => p.Id == id);
                var person = new Person(id, name);
                person.PossessionCount = cnt;
                People.Add(person);
            }
            else
            {
                Console.WriteLine("I: Operation canceled.");
            }
        }
        else
        {
            var person = new Person(id, name);
            person.PossessionCount = cnt;
            People.Add(person);
        }
    }

    private void UserQueryHandler(ushort id)
    {
        var person = People.Find(p => p.Id == id);
        if (person is null)
        {
            Console.WriteLine("E: Failed to query user data. User data not found.");
        }
        else
        {
            Console.WriteLine($"I: User data found. \n" + 
                              $"   USERDATA{{{person.Id},{person.Name},{person.PossessionCount}}}");
        }
    }

    private void StationQueryHandler(string station)
    {
        var s = Stations.Find(s => s.Name.Equals(station, StringComparison.Ordinal));
        if (s is null)
        {
            Console.WriteLine("E: Failed to query station data. Station data not found.");
        }
        else
        {
            Console.WriteLine("=== STATION ===");
            Console.WriteLine($"{{{s.Name},{s.Team0Alias},{s.Team1Alias}}}");
            Console.WriteLine("=== TEAM{0} ===");
            foreach (var (p, c) in s.Team0BetTarget)
            {
                Console.WriteLine($"{p.Id:00000} {p.Name} | {c}/{p.PossessionCount}");
            }

            Console.WriteLine("=== TEAM{1} ===");
            foreach (var (p, c) in s.Team1BetTarget)
            {
                Console.WriteLine($"{p.Id:00000} {p.Name} | {c}/{p.PossessionCount}");
            }

            Console.WriteLine("===============");
        }
    }

    private void BetHandler(ushort id, string station, string team, int betCnt)
    {
        var person = People.Find(p => p.Id == id);
        var s = Stations.Find(s => s.Name.Equals(station, StringComparison.Ordinal));
        if (person is null)
        {
            Console.WriteLine("E: Failed to operate betting. User data not found.");
        }
        else if (s is null)
        {
            Console.WriteLine("E: Failed to operate betting. Station data not found.");
        }
        else
        {
            s.Bet(team, person, betCnt);
            Console.WriteLine("I: Success betting operation.");
        }
    }

    public static void Main()
    {
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.Unicode;

        var prog = new Program();
        while (true)
        {
            prog.Run();
        }
        // ReSharper disable once FunctionNeverReturns
    }
}