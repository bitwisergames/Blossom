using System.Linq;
using Blossom.scripts.controllers.bugs;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public partial class ProjectileController : Node2D, IMoveable
{
    private Node2D _target;
    private int _damage;

    private Timer _lifetimeTimer;

    public float Speed { get; private set; }
    public bool Flying => true;
    public Vector2 TargetPosition { get; private set; }

    private void DamageTarget(Node2D body)
    {
        (body.GetParent() as BaseBugController)!.InflictDamage(_damage);
        DeleteProjectile();
    }

    private void DeleteProjectile()
    {
        QueueFree();
        GetChild(0).GetChildren().OfType<Area2D>().First().BodyEntered -= DamageTarget;
    }

    public void Setup(Node2D target, int damage, float speed)
    {
        _target = target;
        _damage = damage;
        Speed = speed;

        _lifetimeTimer = new Timer();
        AddChild(_lifetimeTimer);

        _lifetimeTimer.Start(10);
        _lifetimeTimer.Timeout += DeleteProjectile;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetChild(0).GetChildren().OfType<Area2D>().First().BodyEntered += DamageTarget;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(_target))
        {
            DeleteProjectile();
            return;
        }

        TargetPosition = _target.GlobalPosition;
    }
}