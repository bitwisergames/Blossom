using System.Collections.Generic;
using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers.flowers;

public abstract partial class BaseFlowerController : Node2D, IFlower
{
    private int _waveSpawned;
    private Sprite2D _sprite;
    protected AnimationPlayer animPlayer;
    protected Area2D area;

    public int Damage => ScaledDamage[Stage];

    public bool Pollinated { get; protected set; } = false;

    public int Stage { get; protected set; } = -1;
    public int Pollen { get; protected set; }
    public float CooldownTimer { get; protected set; }

    public abstract int[] Income { get; }
    public abstract int[] UpgradeCost { get; }
    public abstract int[] ScaledDamage { get; }

    public virtual bool Passive => false;
    public virtual bool Ranged => true;
    public virtual bool CanTargetMultiple => false;
    public virtual float Cooldown => 0f;

    public virtual void Attack(Node2D target)
    {
        if (CanTargetMultiple) return;
        if (!Ranged && (target.GetParent() as IMoveable)!.Flying) return;

        // Spawn Projectile
        var projectileScene = GD.Load<PackedScene>("res://scenes/Projectile.tscn").Instantiate() as Node2D;
        AddChild(projectileScene);

        var projectile = projectileScene as ProjectileController;
        projectile?.Setup(target, 1, 6500f);
        animPlayer.Play(target.GlobalPosition.X > GlobalPosition.X
            ? "FlowerAnimations/AttackRight"
            : "FlowerAnimations/AttackLeft");
        animPlayer.Queue("FlowerAnimations/Idle");
    }

    public virtual void Attack(List<Node2D> targets)
    {
        if (!CanTargetMultiple) return;

        foreach (var target in targets)
        {
            if (!Ranged && (target.GetParent() as IMoveable)!.Flying) continue;
            var parent = target.GetParent();
            (parent as IDamagable)!.InflictDamage(Damage);
        }

        animPlayer.Play("FlowerAnimations/AttackBoth");
        animPlayer.Queue("FlowerAnimations/Idle");
    }

    public virtual bool Pollinate()
    {
        if (Pollinated) return false;
        ++Pollen;

        Pollinated = true;


        return true;
    }

    public void TryUpgrade()
    {
        if (Stage > 3 || (Stage >= 0 && Pollen < UpgradeCost[Stage])) return;
        ++Stage;
        _waveSpawned = GameController.Instance.WaveNumber;
        _sprite.Frame = Stage + 1;
    }

    public void ResetPollination()
    {
        Pollinated = false;
    }

    public override void _Ready()
    {
        area = GetChild(0).GetChildren().OfType<Area2D>().First();
        animPlayer = GetChild(0).GetChildren().OfType<AnimationPlayer>().First();
        animPlayer?.Play("FlowerAnimations/Idle");
        _sprite = GetChild(0).GetChildren().OfType<Sprite2D>().First();
        _sprite.Frame = 0;
        _waveSpawned = GameController.Instance.WaveNumber;
        if (_waveSpawned == 1) TryUpgrade();
    }

    public override void _Process(double delta)
    {
        if (_waveSpawned < GameController.Instance.WaveNumber && GameController.Instance.WaveReadyToStart) TryUpgrade();
        if (Passive) return;
        if (Stage < 0) return;


        CooldownTimer -= (float)delta;

        if (CooldownTimer > 0f) return;

        var enemies = area.GetOverlappingBodies().ToList();
        if (!enemies.Any()) return;

        // Find enemy closest to the hive
        var enemy = enemies.OrderByDescending(enemy => enemy.Position.DistanceSquaredTo(Vector2.Zero)).First();

        Attack(enemy);
        Attack(enemies);

        CooldownTimer = Cooldown;
    }
}