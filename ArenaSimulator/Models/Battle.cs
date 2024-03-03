namespace ArenaSimulator.Models;

public class Battle
{
    public ICollection<Round> Rounds { get; set; } = [];

    public Hero? Winner { get; set; }
}
