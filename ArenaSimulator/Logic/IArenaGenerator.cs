using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public interface IArenaGenerator
{
    Arena GenerateArena(int heroCount);
}