using ArenaSimulator.Commands;
using ArenaSimulator.Exceptions;
using ArenaSimulator.Logic;
using ArenaSimulator.Persistence;
using ArenaSimulator.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ArenaSimulator.Controllers;

public class ArenasController : ApiControllerBase
{
    private readonly SimulatorConfig _simulatorConfig;
    private readonly ApplicationDbContext _dbContext;
    private readonly IArenaGenerator _arenaGenerator;

    public ArenasController(
        IOptions<SimulatorConfig> simulatorConfig,
        ApplicationDbContext applicationDbContext,
        IArenaGenerator arenaGenerator)
    {
        _simulatorConfig = simulatorConfig.Value;
        _dbContext = applicationDbContext;
        _arenaGenerator = arenaGenerator;
    }

    [HttpPost("Generate", Name = "generateArena")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<int> GenerateArena(GenerateArenaCommand command, CancellationToken cancellationToken = default)
    {
        if (command.HeroCount < 1)
            throw new ValidationException($"{nameof(GenerateArenaCommand.HeroCount)} must be greater than 0.");

        if (_simulatorConfig.MaximumAllowedHeroes is not null && _simulatorConfig.MaximumAllowedHeroes < command.HeroCount)
            throw new ValidationException($"Maximum allowed {nameof(GenerateArenaCommand.HeroCount)} id {_simulatorConfig.MaximumAllowedHeroes}.");

        var arena = _arenaGenerator.GenerateArena(command.HeroCount);

        _dbContext.Arenas.Add(arena);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return arena.Id;
    }
}