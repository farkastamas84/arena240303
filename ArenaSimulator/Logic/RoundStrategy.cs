using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public class RoundStrategy : IRoundStrategy
{
    private readonly IRandomGenerator _randomGenerator;
    private readonly IFightSolver _fightSolver;

    private Arena _arena = null!;

    private int HeroCount => _arena.Heroes.Count;

    public RoundStrategy(
        IRandomGenerator randomGenerator,
        IFightSolver fightSolver)
    {
        _randomGenerator = randomGenerator;
        _fightSolver = fightSolver;
    }

    public Round PlayRound(Arena arena)
    {
        _arena = arena;

        CheckHeroCount();

        var (attacker, defender) = DrawHeroes();

        var attackerLogEntry = new HeroLogEntry(attacker);
        var defenderLogEntry = new HeroLogEntry(defender);

        Fight(attacker, defender);

        attackerLogEntry.RecordNewHealth(attacker);
        defenderLogEntry.RecordNewHealth(defender);

        RemoveCorpses(attacker, defender);

        var restingHeroes = _arena.Heroes.Where(x => x != attacker && x != defender);
        HealHeroes(restingHeroes);

        return new Round(attackerLogEntry, defenderLogEntry);
    }

    private void CheckHeroCount()
    {
        if (HeroCount <= 1)
            throw new InvalidOperationException("At least two heroes are needed for a new round.");
    }

    private (Hero attacker, Hero defender) DrawHeroes()
    {
        var attackerIndex = _randomGenerator.Next(HeroCount);
        var defenderIndex = _randomGenerator.Next(HeroCount - 1);

        if (attackerIndex <= defenderIndex)
            defenderIndex++;

        var attacker = _arena.Heroes[attackerIndex];
        var defender = _arena.Heroes[defenderIndex];

        return (attacker, defender);
    }

    private void Fight(Hero attacker, Hero defender) => _fightSolver.SolveFight(attacker, defender);

    private void HealHeroes(IEnumerable<Hero> heroes)
    {
        foreach (var hero in heroes)
            hero.Heal(10);
    }

    private void RemoveCorpses(params Hero[] heroes)
    {
        foreach (var hero in heroes)
            RemoveIfDead(hero);
    }

    private void RemoveIfDead(Hero hero)
    {
        if (!hero.IsAlive)
            _arena.Heroes.Remove(hero);
    }
}