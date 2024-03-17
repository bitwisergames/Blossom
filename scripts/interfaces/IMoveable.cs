namespace Blossom.scripts.interfaces;

public interface IMoveable
{
    /// <summary>The movement speed</summary>
    float Speed { get; set; }

    /// <summary>Does this fly</summary>
    bool Flying { get; }
}