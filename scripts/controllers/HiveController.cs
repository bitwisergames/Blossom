using System.Collections.Generic;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public partial class HiveController : Sprite2D, IDamagable, IDamaging
{
    public static HiveController Instance { get; private set; }

    public int Health { get; private set; } = 100;
    public int MaxHealth => 100;
    public int Damage => 5;
    public float Cooldown => 2;
    public float CooldownTimer => 2;
    public bool Ranged => false;

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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance ??= this;
    }

    public void Attack(IDamagable target)
    {
        target.InflictDamage(Damage);
    }
}