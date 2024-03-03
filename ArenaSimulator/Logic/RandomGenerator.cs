namespace ArenaSimulator.Logic;

public class RandomGenerator : IRandomGenerator
{
    private readonly Random _random;

    public RandomGenerator()
    {
        _random = new Random();
    }

    public int Next(int maxValue) => _random.Next(maxValue);
}