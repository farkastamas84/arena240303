using ArenaSimulator.Controllers;
using ArenaSimulator.Exceptions;
using ArenaSimulator.Logic;
using ArenaSimulator.Setup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace ArenaSimulator.UnitTests;

public class ArenasControllerTests : ControllerTestBase
{
    private static readonly SimulatorConfig _simulatorConfig = new()
    {
        MaximumAllowedHeroes = 100,
    };

    private readonly ArenasController _arenasController;

    public ArenasControllerTests() : base()
    {
        var configMock = new Mock<IOptions<SimulatorConfig>>();
        configMock.Setup(x => x.Value).Returns(_simulatorConfig);

        var arenaGenerator = new ArenaGenerator(new RandomGenerator(), new HeroFactory());

        _arenasController = new ArenasController(configMock.Object, _dbContext, arenaGenerator);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WhenHeroCountIsLessThanOne_ThenValidationExceptionIsThrown(int heroCount)
    {
        var generateArena = () => _arenasController.GenerateArena(new() { HeroCount = heroCount });

        Assert.ThrowsAsync<ValidationException>(generateArena);
    }

    [Fact]
    public void WhenHeroCountExceedsMaximumAllowedValue_ThenValidationExceptionIsThrown()
    {
        var generateArena = () => _arenasController.GenerateArena(new() { HeroCount = _simulatorConfig.MaximumAllowedHeroes!.Value + 1 });

        Assert.ThrowsAsync<ValidationException>(generateArena);
    }

    [Fact]
    public async Task WhenArenaIsGenerated_ThenItIsPersisted()
    {
        var heroCount = 10;

        var arenaId = await _arenasController.GenerateArena(new() { HeroCount = heroCount });

        var arena = await _dbContext.Arenas.AsNoTracking().Include(x => x.Heroes).SingleOrDefaultAsync(x => x.Id == arenaId);

        Assert.NotNull(arena);
        Assert.Equal(heroCount, arena.Heroes.Count);
    }
}