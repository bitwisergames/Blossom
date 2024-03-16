using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.components;

public partial class MovementComponent : CharacterBody2D, IMoveable
{
    private AnimationPlayer _animPlayer;

    public float Speed => 3000f;
    public bool Flying => false;

    public Vector2 TargetPosition { private get; set; }

    public override void _Ready()
    {
        _animPlayer = GetChildren().OfType<AnimationPlayer>().First();
        _animPlayer.Play("Move");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (GlobalPosition.DistanceSquaredTo(TargetPosition) <= 0.1f) return;

        var dir = GlobalPosition.DirectionTo(TargetPosition);
        Velocity = dir.Normalized() * Speed * (float)delta;

        MoveAndSlide();
    }
}