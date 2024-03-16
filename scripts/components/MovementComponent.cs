using System.Linq;
using Godot;

namespace Blossom.scripts.components;

public partial class MovementComponent : CharacterBody2D
{
    private const float Speed = 3000.0f;
    private AnimationPlayer _animPlayer;

    public Vector2 TargetPosition { private get; set; }

    public override void _Ready()
    {
        _animPlayer = GetChildren().OfType<AnimationPlayer>().First();
        _animPlayer.Play("Walk");
    }

    public override void _PhysicsProcess(double delta)
    {
        var dir = GlobalPosition.DirectionTo(TargetPosition);
        Velocity = dir.Normalized() * Speed * (float)delta;

        MoveAndSlide();
    }
}