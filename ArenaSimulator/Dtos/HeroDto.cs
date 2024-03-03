using ArenaSimulator.Models;

namespace ArenaSimulator.Dtos;

public class HeroDto
{
    public int Id { get; init; }

    public HeroType Type { get; init; }

    public decimal Health { get; init; }
}