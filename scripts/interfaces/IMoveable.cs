using Godot;

namespace Blossom.scripts.interfaces;

public interface IMoveable
{
    /// <summary>The movement speed</summary>
    float Speed { get; }

    /// <summary>Does this fly</summary>
    bool Flying { get; }

    public Vector2 TargetPosition { get; }
}