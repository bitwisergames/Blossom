using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public abstract partial class BaseFlowerController : Node2D, IFlower
{
    private Area2D _area2D;

    public bool Pollinated { get; protected set; } = false;
    public int Stage { get; protected set; } = -1;
    public int Pollen { get; protected set; }
    public float CooldownTimer { get; protected set; }

    public abstract int[] Income { get; }
    public abstract int[] Upgrade { get; }
    public abstract string SpritePath { get; }

    public virtual bool Passive => false;
    public virtual bool Ranged => true;
    public virtual int Damage => 0;
    public virtual float Cooldown => 0f;
    public virtual float Radius => 0f;

    /// <summary>Called when this can attack</summary>
    public virtual void Attack(IDamagable target)
    {
        target.InflictDamage(Damage);
    }

    public override void _Ready()
    {
        _area2D = GetChildren().OfType<Area2D>().First();
        var shape = _area2D.GetChildren().OfType<CollisionShape2D>().First().Shape as CircleShape2D;

        shape!.Radius = Radius;
    }

    public override void _Process(double delta)
    {
        if (Passive) return;

        CooldownTimer -= (float)delta;

        if (Cooldown > 0f) return;

        // Find enemy closest to the hive
        var enemy = _area2D.GetOverlappingBodies()
            .OrderBy(enemy => enemy.Position.DistanceSquaredTo(Vector2.Zero)).First();
        Attack(enemy.GetChildren().OfType<IDamagable>().First());
        CooldownTimer = Cooldown;
    }
}