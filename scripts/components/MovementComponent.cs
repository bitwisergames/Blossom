using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.components;

public partial class MovementComponent : CharacterBody2D, IMoveable
{
    private float _speed = 3000f;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public bool Flying => false;

    public Vector2 TargetPosition;

    public override void _PhysicsProcess(double delta)
    {
        if (GlobalPosition.DistanceSquaredTo(TargetPosition) <= 0.1f) return;

        var dir = GlobalPosition.DirectionTo(TargetPosition);
        Velocity = dir.Normalized() * Speed * (float)delta;

        MoveAndSlide();
    }
}