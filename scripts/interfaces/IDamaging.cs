namespace Blossom.scripts.interfaces;

public interface IDamaging
{
    /// <summary>The damage that this does</summary>
    int Damage { get; }

    /// <summary>The max attack cooldown</summary>
    float Cooldown { get; }

    /// <summary>The timer for the cooldown</summary>
    float CooldownTimer { get; }

    /// <summary>Is this ranged</summary>
    /// <remarks>If it's not ranged, it's AOE</remarks>
    bool Ranged { get; }

    /// <summary>Inflict damage on target</summary>
    void Attack(IDamagable target);
}