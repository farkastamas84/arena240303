namespace ArenaSimulator.Models;

public class HeroLogEntry
{
    public Hero Hero { get; private set; }

    public decimal OriginalHealth { get; private set; }

    public decimal NewHealth { get; private set; }

    public HeroLogEntry(Hero hero)
    {
        Hero = hero;
        OriginalHealth = hero.Health;
    }

    public void RecordNewHealth(Hero hero)
    {
        if (hero != Hero)
            throw new ArgumentException("Hero does not match the original hero of this log entry", nameof(hero));

        NewHealth = hero.Health;
    }
}