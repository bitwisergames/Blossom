using System;
using System.Linq;
using Blossom.scripts.components;
using Blossom.scripts.controllers.@base;
using Godot;

namespace Blossom.scripts;

public partial class BugSpawner : Node2D
{
    private const int Radius = 750;

    private double _countdown = 5;
    private int _enemiesPerWave = 10;
    private int _enemiesSpawned;

    private Random _rand;

    private Vector2 GetRandomPointAlongCircle()
    {
        var theta = _rand.NextDouble() * Mathf.Tau;

        return new Vector2((float)Mathf.Cos(theta), (float)Mathf.Sin(theta)) * Radius;
    }

    private void SpawnEnemy(Variant script)
    {
        var enemyScene = GD.Load<PackedScene>("res://scenes/characters/Enemy.tscn");

        // Will eventually be variable as to which enemy is being spawned
        var enemyInstance = enemyScene.Instantiate() as Node2D;

        enemyInstance!.GlobalPosition = GetRandomPointAlongCircle();
        if (enemyInstance!.GlobalPosition.X < 0) enemyInstance.Scale = new Vector2(-1, 1);

        enemyInstance.GetChildren().OfType<Sprite2D>().First().Texture = GD.Load<Texture2D>(info.SpriteResPath);

        AddChild(enemyInstance);

        var movementComponent = enemyInstance as MovementComponent;
        movementComponent!.TargetPosition = Vector2.Zero;

        enemyInstance.GetChildren().OfType<BaseBugController>().First()
            .Setup(info);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _rand = new Random();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_enemiesSpawned == _enemiesPerWave)
            // Setup new wave
            return;

        if (_countdown <= 0)
        {
            var name = "";
            var GD.Load<CSharpScript>($"res://scripts/controllers/{name}.cs").New();
            SpawnEnemy(new BaseBugController.EnemyInfo(
                1,
                "res://assets/sprites/Ant.png",
                1,
                2f,
                18f,
                false,
                false
            ));
            ++_enemiesSpawned;

            _countdown = 1.5;
        }

        _countdown -= delta;
    }
}