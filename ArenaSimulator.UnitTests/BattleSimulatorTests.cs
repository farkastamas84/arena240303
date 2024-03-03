using ArenaSimulator.Exceptions;
using ArenaSimulator.Logic;
using ArenaSimulator.Models;

namespace ArenaSimulator.UnitTests;

public class BattleSimulatorTests
{
    private readonly ArenaGenerator _arenaGenerator;
    private readonly BattleSimulator _battleSimulator;

    public BattleSimulatorTests()
    {
        var randomGenerator = new RandomGenerator();

        _arenaGenerator = new ArenaGenerator(randomGenerator, new HeroFactory());

        var roundStrategy = new RoundStrategy(randomGenerator, new FightSolver(randomGenerator));
        _battleSimulator = new BattleSimulator(roundStrategy);
    }

    [Fact]
    public void WhenRoundLimitExceeded_ThenExceptionIsThrown()
    {
        var arena = _arenaGenerator.GenerateArena(4);

        var simulateBattle = () => _battleSimulator.SimulateBattle(arena, 1);

        Assert.Throws<BattleException>(simulateBattle);
    }

    [Fact]
    public void WhenThereIsOnlyOneHero_ThenTheyWinWithoutAFight()
    {
        var arena = _arenaGenerator.GenerateArena(1);

        var battle = _battleSimulator.SimulateBattle(arena);

        Assert.Empty(battle.Rounds);
        Assert.NotNull(battle.Winner);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(100)]
    public void WhenThereAreMoreThanOneHeroes_ThenHistoryContainsAllHeroes(int heroCount)
    {
        var arena = _arenaGenerator.GenerateArena(heroCount);

        var battle = _battleSimulator.SimulateBattle(arena);

        var attackers = battle.Rounds.Select(x => x.Attacker.Hero).Distinct();
        var defenders = battle.Rounds.Select(x => x.Defender.Hero).Distinct();
        List<Hero> winners = battle.Winner is not null ? [battle.Winner] : [];
        var fighters = attackers.Union(defenders).Union(winners).Distinct();

        Assert.Equal(heroCount, fighters.Count());
    }
}