namespace ArenaSimulator.Dtos;

public class BattleDto
{
    public int RoundCount => Rounds.Count;

    public List<RoundDto> Rounds { get; init; } = [];

    public HeroDto? Winner { get; set; }
}