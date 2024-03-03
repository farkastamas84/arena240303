namespace ArenaSimulator.Models;

public class Hero
{
    private readonly decimal _maxHealth;

    public int Id { get; private set; }

    public HeroType Type { get; private set; }

    public decimal Health { get; private set; }

    public decimal MaxHealth => _maxHealth;

    public bool IsAlive => 0 < Health;

    public Hero(HeroType type, decimal health)
    {
        _maxHealth = health;

        Type = type;
        Health = health;
    }

    public void Wound(decimal points)
    {
        CheckIsAlive();

        Health -= points;

        if (Health < _maxHealth / 4)
            Kill();
    }

    public void Heal(decimal points)
    {
        CheckIsAlive();

        Health += points;

        if (_maxHealth < Health)
            Health = _maxHealth;
    }

    public void Kill()
    {
        CheckIsAlive();

        Health = 0;
    }

    private void CheckIsAlive()
    {
        if (!IsAlive)
            throw new InvalidOperationException("Hero is already dead.");
    }
}