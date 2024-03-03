using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public interface IFightSolver
{
    void SolveFight(Hero attacker, Hero defender);
}