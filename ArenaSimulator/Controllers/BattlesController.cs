using ArenaSimulator.Commands;
using ArenaSimulator.Dtos;
using ArenaSimulator.Exceptions;
using ArenaSimulator.Logic;
using ArenaSimulator.Persistence;
using ArenaSimulator.Setup;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ArenaSimulator.Controllers;

public class BattlesController : ApiControllerBase
{
    private readonly SimulatorConfig _simulatorConfig;
    private readonly ApplicationDbContext _dbContext;
    private readonly IBattleSimulator _battleSimulator;
    private readonly IMapper _mapper;

    public BattlesController(
        IOptions<SimulatorConfig> simulatorConfig,
        ApplicationDbContext dbContext,
        IBattleSimulator battleSimulator,
        IMapper mapper)
    {
        _simulatorConfig = simulatorConfig.Value;
        _dbContext = dbContext;
        _battleSimulator = battleSimulator;
        _mapper = mapper;
    }

    [HttpPut("Simulate", Name = "simulateBattle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<BattleDto> SimulateBattle(SimulateBattleCommand command, CancellationToken cancellationToken = default)
    {
        if (command.ArenaId <= 0)
            throw new ValidationException($"{nameof(SimulateBattleCommand.ArenaId)} must be greater than 0.");

        var arena = await _dbContext.Arenas
            .AsNoTracking()
            .Include(x => x.Heroes)
            .SingleOrDefaultAsync(x => x.Id == command.ArenaId, cancellationToken);

        if (arena is null)
            throw new NotFoundException($"Arena not found with Id {command.ArenaId}.");

        var battle = _battleSimulator.SimulateBattle(arena, _simulatorConfig.MaximumAllowedRounds);

        var battleDto = _mapper.Map<BattleDto>(battle);

        return battleDto;
    }
}