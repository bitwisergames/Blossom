using Blossom.scripts.components;
using Blossom.scripts.controllers.bugs;
using Godot;

namespace Blossom.scripts;

public partial class Projectile : Area2D
{
    private MovementComponent _movementComponent;
    private Node2D _target;
    private int _damage;

    private Timer _lifetimeTimer;

    private void DamageTarget(Node2D body)
    {
        (body.GetParent() as BaseBugController)!.InflictDamage(_damage);
        DeleteProjectile();
    }

    private void DeleteProjectile()
    {
        GetParent().QueueFree();
        BodyEntered -= DamageTarget;
    }

    public void Setup(Node2D target, int damage)
    {
        _target = target;
        _damage = damage;

        _lifetimeTimer = new Timer();
        AddChild(_lifetimeTimer);

        _lifetimeTimer.Start(10);
        _lifetimeTimer.Timeout += DeleteProjectile;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _movementComponent = GetParent<MovementComponent>();
        _movementComponent.Speed = 5500;

        BodyEntered += DamageTarget;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(_movementComponent) || !IsInstanceValid(_target))
        {
            DeleteProjectile();
            return;
        }

        _movementComponent.TargetPosition = _target.GlobalPosition;
    }
}