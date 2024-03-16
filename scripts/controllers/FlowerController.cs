using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Blossom.scripts.controllers;

public partial class FlowerController : Node2D
{
    private float _attackCooldown;
    private float _attackCooldownMax;
    private int _damage;
    private bool _isRanged;

    private Area2D _area2D;

    private List<CharacterBody2D> _enemiesInRange = new();

    public void OnBodyEntered(Node2D body)
    {
        _enemiesInRange.Add(body as CharacterBody2D);
    }

    public void OnBodyExited(Node2D body)
    {
        _enemiesInRange.Remove(body as CharacterBody2D);
    }

    public void Setup(FlowerInfo info)
    {
        _damage = info.Damage;
        _attackCooldown = 0;
        _attackCooldownMax = info.AttackCooldown;
        _isRanged = info.IsRanged;

        _area2D = GetChildren().OfType<Area2D>().First();
        var shape = _area2D.GetChildren().OfType<CollisionShape2D>().First().Shape as CircleShape2D;

        shape!.Radius = info.AttackRadius;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_attackCooldown <= 0)
        {
            // Find enemy closest to the hive
            var minDistance = float.PositiveInfinity;
            var index = -1;

            for (var i = 0; i < _enemiesInRange.Count; ++i)
            {
                var distance = _enemiesInRange[i].Position.DistanceSquaredTo(HiveController.Instance.Position);

                if (distance < minDistance)
                {
                    index = i;
                    minDistance = distance;
                }
            }

            if (index < 0) return;

            _enemiesInRange[index].GetChildren().OfType<EnemyController>().First().InflictDamage(_damage);
            _enemiesInRange.RemoveAt(index);

            _attackCooldown = _attackCooldownMax;
        }

        _attackCooldown -= (float)delta;
    }

    public record FlowerInfo(
        string SpriteResPath,
        int Damage,
        float AttackCooldown,
        float AttackRadius,
        bool IsRanged
    );
}