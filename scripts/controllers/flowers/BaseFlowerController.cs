using System.Collections.Generic;
using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers.flowers;

public abstract partial class BaseFlowerController : Node2D, IFlower
{
    protected Area2D Area2D;

    public bool Pollinated { get; protected set; } = false;

    public int Stage { get; protected set; } = -1;
    public int Pollen { get; protected set; }
    public float CooldownTimer { get; protected set; }

    public abstract int[] Income { get; }
    public abstract int[] Upgrade { get; }

    public virtual bool Passive => false;
    public virtual bool Ranged => true;
    public virtual int Damage => 0;
    public virtual float Cooldown => 0f;

    public virtual bool Pollinate()
    {
        if (Pollinated) return false;
        ++Pollen;
        // Give pollen to the player in the amount of Income[Stage]
        // I don't know if we should upgrade here, or the beginning of next round
        return true;
    }

    /// <summary>Called when this can attack</summary>
    public virtual void Attack(Node2D target)
    {
        (target as IDamagable)?.InflictDamage(Damage);
    }

    public virtual void Attack(List<Node2D> targets)
    {
        foreach (var target in targets) (target as IDamagable)?.InflictDamage(Damage);
    }

    public override void _Ready()
    {
        Area2D = GetChild(0).GetChildren().OfType<Area2D>().First();
    }

    public override void _Process(double delta)
    {
        if (Passive) return;

        CooldownTimer -= (float)delta;

        if (Cooldown > 0f) return;

        var enemies = Area2D.GetOverlappingBodies().Where(enemy => enemy is IEnemy).ToList();
        if (!enemies.Any()) return;

        if (Ranged)
        {
            // Find enemy closest to the hive
            var enemy = enemies.OrderBy(enemy => enemy.Position.DistanceSquaredTo(Vector2.Zero)).First();
            Attack(enemy);
        }
        else
        {
            Attack(enemies);
        }

        CooldownTimer = Cooldown;
    }
}