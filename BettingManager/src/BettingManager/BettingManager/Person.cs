namespace BettingManager;

public class Person
{
    private int _possessionCount;

    public Person(ushort id, string name)
    {
        Id = id;
        Name = name;
    }

    public ushort Id { get; }
    public string Name { get; }
    public ref int PossessionCount => ref _possessionCount;
}