namespace Blossom.scripts.interfaces;

public interface IEnemy : IDamaging, IDamagable, ISpritable
{
    /// <summary>Does this fly</summary>
    bool Flying { get; }
}