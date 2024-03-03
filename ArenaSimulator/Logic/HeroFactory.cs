using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public class HeroFactory : IHeroFactory
{
    public Hero CreateHero(HeroType heroType)
    {
        var maxHealth = heroType switch
        {
            HeroType.Archer => 100,
            HeroType.Swordsman => 120,
            HeroType.Horseman => 150,
            _ => throw new NotImplementedException($"Unhandled hero type: {heroType}."),
        };

        var hero = new Hero(heroType, maxHealth);

        return hero;
    }
}
