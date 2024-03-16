using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers.@base;

public abstract class BaseBugController : Node2D, IEnemy
{
    public float CooldownTimer { get; protected set; }
    public int Health { get; protected set; }

    public abstract string SpritePath { get; }
    public abstract int Damage { get; }
    public abstract float Cooldown { get; }
    public abstract float Radius { get; }

    public virtual bool Flying => false;
    public virtual bool Ranged => false;

    public virtual void Attack(IDamagable target)
    {
        target.InflictDamage(Damage);
    }

    public void InflictDamage(int amount)
    {
        Health -= amount;

        if (Health > 0) return;

        Die();
    }

    public void Die()
    {
        GD.Print("Bug ded");
        GetParent().QueueFree();
    }

    public override void _Ready()
    {
        var area2D = GetParent().GetChildren().OfType<Area2D>().First();
        var shape = area2D.GetChildren().OfType<CollisionShape2D>().First().Shape as CircleShape2D;

        shape!.Radius = Radius;
    }

    public override void _Process(double delta)
    {
        CooldownTimer -= (float)delta;

        if (CooldownTimer > 0f) return;
        Attack(HiveController.Instance);
        CooldownTimer = Cooldown;
    }
}