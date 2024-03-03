namespace ArenaSimulator.Models;

public class Arena
{
    public int Id { get; private set; }

    public IList<Hero> Heroes { get; private set; } = [];
}
