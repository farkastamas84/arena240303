using ArenaSimulator.Logic;
using ArenaSimulator.Models;

namespace ArenaSimulator.UnitTests;

public class RoundStrategyTests
{
    private readonly ArenaGenerator _arenaGenerator;
    private readonly RoundStrategy _roundStrategy;

    public RoundStrategyTests()
    {
        var randomGenerator = new RandomGenerator();

        _arenaGenerator = new ArenaGenerator(randomGenerator, new HeroFactory());

        _roundStrategy = new RoundStrategy(randomGenerator, new FightSolver(randomGenerator));
    }

    [Fact]
    public void GivenAllHeroesAreInjured_WhenANewRoundIsPlayed_ThenRestingHeroesHeal()
    {
        // Arrange
        var arena = new Arena();
        for (int i = 0; i < 5; i++)
        {
            var hero = new Hero(HeroType.Swordsman, 100);
            hero.Wound(40);
            arena.Heroes.Add(hero);
        }

        // Act
        var round = _roundStrategy.PlayRound(arena);

        // Assert
        foreach (var hero in arena.Heroes)
        {
            if (hero != round.Attacker.Hero && hero != round.Defender.Hero)
                Assert.Equal(70, hero.Health);
        }
    }

    [Fact]
    public void GivenANewArena_WhenRoundsArePlayed_ThenCorpsesAreRemoved()
    {
        // Arrange
        var arena = _arenaGenerator.GenerateArena(100);

        while (1 < arena.Heroes.Count)
        {
            // Act
            var round = _roundStrategy.PlayRound(arena);

            // Assert
            if (!round.Attacker.Hero.IsAlive)
                Assert.DoesNotContain(round.Attacker.Hero, arena.Heroes);
            if (!round.Defender.Hero.IsAlive)
                Assert.DoesNotContain(round.Defender.Hero, arena.Heroes);
        }
    }
}