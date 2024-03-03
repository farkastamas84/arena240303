using ArenaSimulator.Models;

namespace ArenaSimulator.Logic;

public class FightSolver : IFightSolver
{
    private readonly IRandomGenerator _randomGenerator;

    public FightSolver(IRandomGenerator randomGenerator)
    {
        _randomGenerator = randomGenerator;
    }

    public void SolveFight(Hero attacker, Hero defender)
    {
        switch (attacker.Type)
        {
            case HeroType.Archer:

                switch (defender.Type)
                {
                    case HeroType.Horseman:
                        if (_randomGenerator.Next(5) < 2)
                            defender.Kill();
                        break;

                    case HeroType.Swordsman:
                        defender.Kill();
                        break;

                    case HeroType.Archer:
                        defender.Kill();
                        break;

                    default:
                        throw NewNotImplementedCombination(attacker, defender);
                }
                break;

            case HeroType.Swordsman:
                switch (defender.Type)
                {
                    case HeroType.Horseman:
                        break;

                    case HeroType.Swordsman:
                        defender.Kill();
                        break;

                    case HeroType.Archer:
                        defender.Kill();
                        break;

                    default:
                        throw NewNotImplementedCombination(attacker, defender);
                }
                break;

            case HeroType.Horseman:
                switch (defender.Type)
                {
                    case HeroType.Horseman:
                        defender.Kill();
                        break;

                    case HeroType.Swordsman:
                        attacker.Kill();
                        break;

                    case HeroType.Archer:
                        defender.Kill();
                        break;

                    default:
                        throw NewNotImplementedCombination(attacker, defender);
                }
                break;

            default:
                throw new NotImplementedException($"Unhandled attacker type: {attacker.Type}.");
        }

        if (attacker.IsAlive)
            WoundHero(attacker);

        if (defender.IsAlive)
            WoundHero(defender);
    }

    private static void WoundHero(Hero hero) => hero.Wound(hero.Health / 2);

    private static NotImplementedException NewNotImplementedCombination(Hero attacker, Hero defender) =>
        new($"Unhandled attack: {attacker.Type} -> {defender.Type}.");
}