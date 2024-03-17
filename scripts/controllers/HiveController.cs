using System.Collections.Generic;
using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public partial class HiveController : Node2D, IDamagable, IDamaging
{
    public static HiveController Instance { get; private set; }

    private AnimationPlayer _animPlayer;

    public int Health { get; private set; } = 100;
    public int MaxHealth => 100;
    public int Damage => 5;
    public float Cooldown => 2;
    public float CooldownTimer => 2;
    public bool Ranged => false;
    public bool CanTargetMultiple => true;

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

        if (Health <= 0) Die();
    }

    public void Die()
    {
        GD.Print("Ded");
    }

    public void SetReady()
    {
        _animPlayer?.Play("Ready");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance ??= this;

        _animPlayer = GetChildren().OfType<AnimationPlayer>().First();

        _animPlayer?.Play("Ready");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!GameController.Instance.WaveReadyToStart) return;
        if (@event is not InputEventMouseButton mouseEvent || !mouseEvent.IsReleased()) return;
        if (mouseEvent.ButtonIndex != MouseButton.Left) return;
        if (GetGlobalMousePosition().DistanceSquaredTo(Vector2.Zero) > 4900) return;

        GameController.Instance.StartWave();
        _animPlayer?.Play("Idle");
    }
}