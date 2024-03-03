namespace ArenaSimulator.Dtos;

public class RoundDto
{
    public HeroLogEntryDto Attacker { get; init; } = null!;

    public HeroLogEntryDto Defender { get; init; } = null!;
}