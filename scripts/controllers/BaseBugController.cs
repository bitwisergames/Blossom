using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public abstract partial class BaseBugController : Node2D, IEnemy
{
    private Area2D _area2D;

    public float CooldownTimer { get; protected set; }
    public int Health { get; protected set; }

    public abstract int MaxHealth { get; }
    public abstract int Damage { get; }
    public abstract float Cooldown { get; }

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
        _area2D = GetChild(0).GetChildren().OfType<Area2D>().First();
    }

    public override void _Process(double delta)
    {
        CooldownTimer -= (float)delta;

        if (CooldownTimer > 0f) return;
        if (_area2D.GetOverlappingBodies().Count == 0) return;

        Attack(HiveController.Instance);
        CooldownTimer = Cooldown;
    }
}