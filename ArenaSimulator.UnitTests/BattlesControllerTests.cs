using ArenaSimulator.Controllers;
using ArenaSimulator.Exceptions;
using ArenaSimulator.Logic;
using ArenaSimulator.Setup;
using Microsoft.Extensions.Options;
using Moq;

namespace ArenaSimulator.UnitTests;

public class BattlesControllerTests : ControllerTestBase
{
    private static readonly SimulatorConfig _simulatorConfig = new()
    {
        MaximumAllowedRounds = 1000,
    };

    private readonly BattlesController _battlesController;
    private readonly ArenaGenerator _arenaGenerator;

    public BattlesControllerTests() : base()
    {
        var configMock = new Mock<IOptions<SimulatorConfig>>();
        configMock.Setup(x => x.Value).Returns(_simulatorConfig);
        var randomGenerator = new RandomGenerator();

        _arenaGenerator = new ArenaGenerator(randomGenerator, new HeroFactory());

        var roundStrategy = new RoundStrategy(randomGenerator, new FightSolver(randomGenerator));
        var battleSimulator = new BattleSimulator(roundStrategy);
        _battlesController = new BattlesController(configMock.Object, _dbContext, battleSimulator, _mapper);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WhenArenaIdIsLessThanOne_ThenValidationExceptionIsThrown(int arenaId)
    {
        var simulateBattle = () => _battlesController.SimulateBattle(new() { ArenaId = arenaId });

        Assert.ThrowsAsync<ValidationException>(simulateBattle);
    }

    [Fact]
    public void WhenArenaDoesNotExist_ThenNotFoundExceptionIsThrown()
    {
        var simulateBattle = () => _battlesController.SimulateBattle(new() { ArenaId = 0 });

        Assert.ThrowsAsync<NotFoundException>(simulateBattle);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task WhenArenaExists_ThenBattleHistoryIsReturned(int heroCount)
    {
        var arena = _arenaGenerator.GenerateArena(heroCount);
        _dbContext.Add(arena);
        _dbContext.SaveChanges();

        var battleDto = await _battlesController.SimulateBattle(new() { ArenaId = arena.Id });

        Assert.NotNull(battleDto);
        Assert.Equal(battleDto.RoundCount, battleDto.Rounds.Count);
    }
}