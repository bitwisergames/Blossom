using System.Linq;
using Godot;

namespace Blossom.scripts.controllers;

public partial class EnemyController : Node2D
{
    private float _attackCooldown;
    private float _attackCooldownMax;
    private int _damage;

    private int _health;

    private bool _isFlying;
    private bool _isRanged;

    public void Setup(EnemyInfo info)
    {
        _health = info.Health;

        _damage = info.Damage;
        _attackCooldown = 0;
        _attackCooldownMax = info.AttackCooldown;
        _isRanged = info.IsRanged;

        _isFlying = info.IsFlying;

        var area = GetParent().GetChildren().OfType<Area2D>().First();
        var shape = area.GetChildren().OfType<CollisionShape2D>().First().Shape as CircleShape2D;

        shape!.Radius = info.AttackRadius;
    }

    public void InflictDamage(int amount)
    {
        _health -= amount;

        if (_health <= 0)
        {
            GD.Print("Bug ded");

            GetParent().QueueFree();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_attackCooldown <= 0)
        {
            HiveController.Instance.InflictDamage(_damage);
            _attackCooldown = _attackCooldownMax;
        }

        _attackCooldown -= (float)delta;
    }

    public record EnemyInfo(
        int Health,
        string SpriteResPath,
        int Damage,
        float AttackCooldown,
        float AttackRadius,
        bool IsRanged,
        bool IsFlying
    );
}