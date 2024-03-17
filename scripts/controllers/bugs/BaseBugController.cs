using System.Collections.Generic;
using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers.bugs;

public abstract partial class BaseBugController : Node2D, IBug
{
    private Area2D _area2D;
    private AnimationPlayer _animPlayer;

    public float CooldownTimer { get; protected set; }
    public int Health { get; protected set; }

    public abstract int MaxHealth { get; }
    public abstract int Damage { get; }
    public abstract float Cooldown { get; }

    public virtual float Speed => 3000.0f;
    public virtual bool Flying => false;
    public virtual Vector2 TargetPosition => Vector2.Zero;
    public virtual bool Ranged => false;
    public virtual bool CanTargetMultiple => false;

    public void Attack(Node2D target)
    {
        (target as IDamagable)?.InflictDamage(Damage);
    }

    public void Attack(List<Node2D> targets)
    {
        foreach (var target in targets)
        {
            Attack(target);
        }
    }

    public void InflictDamage(int amount)
    {
        Health -= amount;

        if (Health > 0) return;

        Die();
    }

    public void Die()
    {
        QueueFree();
    }

    public override void _Ready()
    {
        _area2D = GetChild(0).GetChildren().OfType<Area2D>().First();

        _animPlayer = GetChild(0).GetChildren().OfType<AnimationPlayer>().First();
        _animPlayer.Play(Flying ? "BugAnimations/Fly" : "BugAnimations/Walk");

        Health = MaxHealth;
    }

    public override void _Process(double delta)
    {
        CooldownTimer -= (float)delta;

        if (CooldownTimer > 0f) return;

        if (!_area2D.GetOverlappingBodies().Any()) return;

        Attack(HiveController.Instance);

        CooldownTimer = Cooldown;
    }
}