namespace ArenaSimulator.Models;

public class Round
{
    public HeroLogEntry Attacker { get; private set; }

    public HeroLogEntry Defender { get; private set; }

    public Round(HeroLogEntry attackerLogEntry, HeroLogEntry defenderLogEntry)
    {
        Attacker = attackerLogEntry;
        Defender = defenderLogEntry;
    }
}
