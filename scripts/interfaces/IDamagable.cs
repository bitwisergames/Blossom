namespace Blossom.scripts.interfaces;

public interface IDamagable
{
    /// <summary>The amount of health this has left</summary>
    int Health { get; }

    /// <summary>The max health</summary>
    int MaxHealth { get; }

    /// <summary>Inflict X damage on this where X is amount</summary>
    void InflictDamage(int amount);

    /// <summary>Do what is needed when this dies</summary>
    void Die();
}