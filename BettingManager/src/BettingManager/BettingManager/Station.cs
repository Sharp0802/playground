using System.Collections.Concurrent;

namespace BettingManager;

public class Station
{
    public Station(string name, string team0Alias, string team1Alias)
    {
        Name = name;
        Team0Alias = team0Alias;
        Team1Alias = team1Alias;
    }

    public string Name { get; }
    
    public string Team0Alias { get; }
    public ConcurrentDictionary<Person, int> Team0BetTarget { get; } = new();

    public string Team1Alias { get; }
    public ConcurrentDictionary<Person, int> Team1BetTarget { get; } = new();

    public void Bet(string team, Person person, int count)
    {
        if (team.Equals(Team0Alias, StringComparison.Ordinal))
        {
            Team0BetTarget.AddOrUpdate(person, p =>
            {
                p.PossessionCount -= count;
                return count;
            }, (p, old) =>
            {
                if (p.PossessionCount < count)
                    throw new InvalidOperationException();

                p.PossessionCount -= count;
                return old + count;
            });
        }
        else if (team.Equals(Team1Alias, StringComparison.Ordinal))
        {
            Team1BetTarget.AddOrUpdate(person, p =>
            {
                p.PossessionCount -= count;
                return count;
            }, (p, old) =>
            {
                if (p.PossessionCount < count)
                    throw new InvalidOperationException();

                p.PossessionCount -= count;
                return old + count;
            });
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(team));
        }
    }
}