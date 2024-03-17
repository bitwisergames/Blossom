using System.Collections.Generic;
using System.Linq;
using Blossom.scripts.controllers.bugs;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers.flowers;

public abstract partial class BaseFlowerController : Node2D, IFlower
{
    protected Area2D Area;
    private AnimationPlayer _animPlayer;

    public bool Pollinated { get; protected set; } = false;

    public int Stage { get; protected set; } = -1;
    public int Pollen { get; protected set; }
    public float CooldownTimer { get; protected set; }

    public abstract int[] Income { get; }
    public abstract int[] Upgrade { get; }

    public virtual bool Passive => false;
    public virtual bool Ranged => true;

    public virtual void Attack(Node2D target)
    {
        if (!Ranged) return;

        // Spawn Projectile
        var projectileScene = GD.Load<PackedScene>("res://scenes/Projectile.tscn").Instantiate() as Node2D;
        AddChild(projectileScene);

        var projectile = projectileScene!.GetChildren().OfType<Projectile>().First();
        projectile.Setup(target, 1);

        _animPlayer.Play("Attack");
        _animPlayer.Queue("Sway");
    }

    public virtual void Attack(List<Node2D> targets)
    {
        if (Ranged) return;

        foreach (var target in targets)
        {
            var parent = target.GetParent();
            (parent as IDamagable)!.InflictDamage(Damage);
        }
    }

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

    public override void _Ready()
    {
        Area = GetChild(0).GetChildren().OfType<Area2D>().First();
        _animPlayer = GetChild(0).GetChildren().OfType<AnimationPlayer>().First();

        _animPlayer?.Play("Sway");
    }

    public override void _Process(double delta)
    {
        if (Passive) return;

        CooldownTimer -= (float)delta;

        if (CooldownTimer > 0f) return;

        var enemies = Area.GetOverlappingBodies().ToList();
        if (!enemies.Any()) return;

        // Find enemy closest to the hive
        var enemy = enemies.OrderBy(enemy => enemy.Position.DistanceSquaredTo(Vector2.Zero)).First();

        Attack(enemy);
        Attack(enemies);

        CooldownTimer = Cooldown;
    }
}