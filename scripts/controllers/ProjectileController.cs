using System.Linq;
using Blossom.scripts.controllers.bugs;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public partial class ProjectileController : Node2D, IMoveable
{
    private Node2D _target;
    private int _damage;
    private bool _aoe;
    private float _aoeRange;
    private Area2D _area;

    private Timer _lifetimeTimer;

    public float Speed { get; private set; }
    public bool Flying => true;
    public Vector2 TargetPosition { get; private set; }

    private void DamageTarget(Node2D body)
    {
        (body.GetParent() as IDamagable)!.InflictDamage(_damage);
        if (_aoe)
        {
            (_area.GetChild<CollisionShape2D>(0).Shape as CircleShape2D)!.Radius = _aoeRange;
            var enemies = _area.GetOverlappingBodies();
            foreach (var enemy in enemies)
            {
                (enemy.GetParent() as IDamagable)!.InflictDamage(_damage);
            }
        }

        DeleteProjectile();
    }

    private void DeleteProjectile()
    {
        QueueFree();
        _area.BodyEntered -= DamageTarget;
    }

    public void Setup(Node2D target, int damage, float speed, bool aoe = false, float aoeRange = 0.01f)
    {
        _target = target;
        _damage = damage;
        _aoe = aoe;
        _aoeRange = aoeRange;
        Speed = speed;

        _lifetimeTimer = new Timer();
        AddChild(_lifetimeTimer);

        _lifetimeTimer.Start(10);
        _lifetimeTimer.Timeout += DeleteProjectile;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _area = GetChild(0).GetChildren().OfType<Area2D>().First();
        _area.BodyEntered += DamageTarget;
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