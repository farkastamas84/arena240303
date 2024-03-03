using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public interface IBattleSimulator
{
    Battle SimulateBattle(Arena arena, int? maximumAllowedRounds = null);
}