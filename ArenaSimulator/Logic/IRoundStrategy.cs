using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public interface IRoundStrategy
{
    Round PlayRound(Arena arena);
}