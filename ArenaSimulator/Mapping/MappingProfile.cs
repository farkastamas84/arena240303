using ArenaSimulator.Dtos;
using ArenaSimulator.Models;
using AutoMapper;

namespace ArenaSimulator.Mapping;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Battle, BattleDto>(MemberList.Destination);

        CreateMap<Round, RoundDto>(MemberList.Destination);

        CreateMap<Hero, HeroDto>(MemberList.Destination);

        CreateMap<HeroLogEntry, HeroLogEntryDto>(MemberList.Destination);
    }
}
