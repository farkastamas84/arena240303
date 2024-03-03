using ArenaSimulator.Logic;
using ArenaSimulator.Models;
using Moq;

namespace ArenaSimulator.UnitTests;

public class FightSolverTests
{
    private readonly Mock<IRandomGenerator> _randomGeneratorMock;
    private readonly HeroFactory _heroFactory;
    private readonly FightSolver _fightSolver;

    public FightSolverTests()
    {
        _randomGeneratorMock = new Mock<IRandomGenerator>();
        _heroFactory = new HeroFactory();
        _fightSolver = new FightSolver(_randomGeneratorMock.Object);
    }

    [Theory]
    [MemberData(nameof(AllCombinations))]
    public void TestAllCombinationsAreImplemented(HeroType attackerType, HeroType defenderType)
    {
        var attacker = _heroFactory.CreateHero(attackerType);
        var defender = _heroFactory.CreateHero(defenderType);

        var exception = Record.Exception(() => _fightSolver.SolveFight(attacker, defender));
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(HeroType.Archer, HeroType.Swordsman, false, true)]
    [InlineData(HeroType.Archer, HeroType.Archer, false, true)]
    [InlineData(HeroType.Swordsman, HeroType.Horseman, false, false)]
    [InlineData(HeroType.Swordsman, HeroType.Swordsman, false, true)]
    [InlineData(HeroType.Swordsman, HeroType.Archer, false, true)]
    [InlineData(HeroType.Horseman, HeroType.Horseman, false, true)]
    [InlineData(HeroType.Horseman, HeroType.Swordsman, true, false)]
    [InlineData(HeroType.Horseman, HeroType.Archer, false, true)]
    public void TestNonRandomizedAttacks(HeroType attackerType, HeroType defenderType, bool attackerShouldDie, bool defenderShouldDie)
    {
        var attacker = _heroFactory.CreateHero(attackerType);
        var attackerOriginalHealth = attacker.Health;
        var defender = _heroFactory.CreateHero(defenderType);
        var defenderOriginalHealth = defender.Health;

        _fightSolver.SolveFight(attacker, defender);

        Assert.Equal(attackerShouldDie, !attacker.IsAlive);
        Assert.Equal(defenderShouldDie, !defender.IsAlive);

        if (!attackerShouldDie)
            Assert.Equal(attackerOriginalHealth / 2, attacker.Health);
        if (!defenderShouldDie)
            Assert.Equal(defenderOriginalHealth / 2, defender.Health);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(3, false)]
    [InlineData(4, false)]
    public void TestArcherAttackingHorseman(int randomNumber, bool horsemanShouldDie)
    {
        var attacker = _heroFactory.CreateHero(HeroType.Archer);
        var attackerOriginalHealth = attacker.Health;
        var defender = _heroFactory.CreateHero(HeroType.Horseman);
        var defenderOriginalHealth = defender.Health;

        _randomGeneratorMock.Setup(x => x.Next(It.IsAny<int>())).Returns(randomNumber);

        _fightSolver.SolveFight(attacker, defender);

        Assert.True(attacker.IsAlive);
        Assert.Equal(horsemanShouldDie, !defender.IsAlive);

        Assert.Equal(attackerOriginalHealth / 2, attacker.Health);

        if (!horsemanShouldDie)
            Assert.Equal(defenderOriginalHealth / 2, defender.Health);
    }

    public static IEnumerable<object[]> AllCombinations =>
        from attackerType in Enum.GetValues<HeroType>()
        from defenderType in Enum.GetValues<HeroType>()
        select new object[] { attackerType, defenderType };
}