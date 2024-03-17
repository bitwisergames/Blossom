using System.Collections.Generic;
using Godot;

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
    /// <remarks>If it's not ranged, it's melee</remarks>
    bool Ranged { get; }

    /// <summary>Does this target one thing or multiple things</summary>
    bool CanTargetMultiple { get; }

    /// <summary>Attack single target</summary>
    void Attack(Node2D target);

    /// <summary>Attack targets in AOE</summary>
    void Attack(List<Node2D> targets);
}