using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.components;

public partial class MovementComponent : CharacterBody2D
{
    private IMoveable _info;

    public override void _Ready()
    {
        _info = GetParent() as IMoveable;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (GlobalPosition.DistanceSquaredTo(_info.TargetPosition) <= 0.1f) return;

        var dir = GlobalPosition.DirectionTo(_info.TargetPosition);
        Velocity = dir.Normalized() * _info.Speed * (float)delta;

        MoveAndSlide();
    }
}