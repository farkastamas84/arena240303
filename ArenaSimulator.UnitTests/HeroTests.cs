using ArenaSimulator.Models;

namespace ArenaSimulator.UnitTests;

public class HeroTests
{
    [Fact]
    public void GivenAHealthyHero_WhenWoundedToQuarterHealth_ThenTheyAreStillAlive()
    {
        // Arrage
        var hero = new Hero(HeroType.Archer, 100);

        // Act
        hero.Wound(hero.Health / 4 * 3 );

        // Assert
        Assert.True(hero.IsAlive);
    }

    [Fact]
    public void GivenAHealthyHero_WhenWoundedBelowQuarterHealth_ThenTheyDie()
    {
        // Arrage
        var hero = new Hero(HeroType.Archer, 100);

        // Act
        hero.Wound(hero.Health / 4 * 3 + 0.0001m);

        // Assert
        Assert.False(hero.IsAlive);
    }

    [Fact]
    public void GivenAWoundedHero_WhenTheyAreHealed_ThenTheirHealthIncreases()
    {
        // Arrage
        var maxHealth = 30;
        var hero = new Hero(HeroType.Archer, maxHealth);

        hero.Wound(20);
        var woundedHealth = hero.Health;
        
        // Act
        hero.Heal(10);

        // Assert
        Assert.Equal(woundedHealth + 10, hero.Health);
    }

    [Fact]
    public void GivenAWoundedHero_WhenTheyAreOverhealed_ThenTheirHealthStopsAtMaximum()
    {
        // Arrage
        var maxHealth = 30;
        var hero = new Hero(HeroType.Archer, maxHealth);

        hero.Wound(20);

        // Act
        hero.Heal(20.0001m);

        // Assert
        Assert.Equal(maxHealth, hero.Health);
    }

    [Fact]
    public void GivenAHero_WhenTheyAreKilled_ThenTheyAreDead()
    {
        // Arrage
        var maxHealth = 10;
        var hero = new Hero(HeroType.Archer, maxHealth);

        // Act
        hero.Kill();

        // Assert
        Assert.False(hero.IsAlive);
    }
}