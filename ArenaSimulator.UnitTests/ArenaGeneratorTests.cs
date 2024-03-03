using ArenaSimulator.Logic;

namespace ArenaSimulator.UnitTests;

public class ArenaGeneratorTests
{
    private readonly ArenaGenerator _arenaGenerator;

    public ArenaGeneratorTests()
    {
        _arenaGenerator = new ArenaGenerator(new RandomGenerator(), new HeroFactory());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(100)]
    public void WhenAnArenaIsGenerated_ThenHeroCountMatchesInput(int heroCount)
    {
        var arena = _arenaGenerator.GenerateArena(heroCount);

        Assert.Equal(arena.Heroes.Count, heroCount);
    }

    [Fact]
    public void WhenAnArenaIsGenerated_ThenAllHeroesHaveMaximumHealth()
    {
        var arena = _arenaGenerator.GenerateArena(100);

        foreach (var hero in arena.Heroes)
            Assert.Equal(hero.MaxHealth, hero.Health);
    }
}