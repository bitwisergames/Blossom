namespace Blossom.scripts.interfaces;

public interface IEnemy : IDamaging, ISpritable
{
    /// <summary>The amount of health this has left</summary>
    int Health { get; }

    /// <summary>Does this fly</summary>
    bool Flying { get; }
}