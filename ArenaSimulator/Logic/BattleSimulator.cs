using ArenaSimulator.Exceptions;
using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public class BattleSimulator : IBattleSimulator
{
    private readonly IRoundStrategy _roundStrategy;

    public BattleSimulator(IRoundStrategy roundStrategy)
    {
        _roundStrategy = roundStrategy;
    }

    public Battle SimulateBattle(Arena arena, int? maximumAllowedRounds = null)
    {
        var battle = new Battle();

        while ((maximumAllowedRounds is null || battle.Rounds.Count < maximumAllowedRounds)
            && 1 < arena.Heroes.Count)
        {
            var round = _roundStrategy.PlayRound(arena);
            battle.Rounds.Add(round);
        }

        if (1 < arena.Heroes.Count)
            throw new BattleException("Maximum allowed rounds reached.");

        battle.Winner = arena.Heroes.SingleOrDefault();

        return battle;
    }
}