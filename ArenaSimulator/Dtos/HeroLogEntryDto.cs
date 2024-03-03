using ArenaSimulator.Models;

namespace ArenaSimulator.Dtos;

public class HeroLogEntryDto
{
    public int HeroId { get; init; }

    public HeroType HeroType { get; init; }

    public decimal OriginalHealth { get; init; }

    public decimal NewHealth { get; init; }
}