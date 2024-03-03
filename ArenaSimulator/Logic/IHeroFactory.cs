using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public interface IHeroFactory
{
    Hero CreateHero(HeroType heroType);
}