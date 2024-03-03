using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public class ArenaGenerator : IArenaGenerator
{
    private readonly IRandomGenerator _randomGenerator;
    private readonly IHeroFactory _heroFactory;

    private static readonly HeroType[] _heroTypes = Enum.GetValues<HeroType>();

    public ArenaGenerator(
        IRandomGenerator randomGenerator,
        IHeroFactory heroFactory)
    {
        _randomGenerator = randomGenerator;
        _heroFactory = heroFactory;
    }

    public Arena GenerateArena(int heroCount)
    {
        var arena = new Arena();

        for (int i = 0; i < heroCount; i++)
        {
            var heroType = GetRandomHeroType();

            var hero = _heroFactory.CreateHero(heroType);

            arena.Heroes.Add(hero);
        }

        return arena;
    }

    private HeroType GetRandomHeroType() => _heroTypes[_randomGenerator.Next(_heroTypes.Length)];
}