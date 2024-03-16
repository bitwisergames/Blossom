namespace Blossom.scripts.interfaces;

public interface IDamaging
{
    /// <summary>The damage that this does</summary>
    int Damage { get; }
    
    /// <summary>The attack cooldown</summary>
    float Cooldown { get; }
    
    /// <summary>The radius in which this can attack</summary>
    float Radius { get; }
    
    /// <summary>Is this ranged</summary>
    /// <remarks>If it's not ranged, it's AOE</remarks>
    bool Ranged { get; }
}