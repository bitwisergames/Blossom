using System;
using System.Collections.Generic;
using Blossom.scripts.components;
using Godot;

namespace Blossom.scripts;

public partial class BugSpawner : Node2D
{
    private const int Radius = 750;

    private double _countdown = 5;
    private int _enemiesPerWave = 10;
    private int _enemiesSpawned;

    private Random _rand;

    private List<PackedScene> _bugScenes = new();

    private Vector2 GetRandomPointAlongCircle()
    {
        var theta = _rand.NextDouble() * Mathf.Tau;

        return new Vector2((float)Mathf.Cos(theta), (float)Mathf.Sin(theta)) * Radius;
    }

    private void SpawnEnemy(PackedScene toSpawn)
    {
        var enemyInstance = toSpawn.Instantiate() as Node2D;

        enemyInstance!.GlobalPosition = GetRandomPointAlongCircle();
        if (enemyInstance!.GlobalPosition.X < 0) enemyInstance.Scale = new Vector2(-1, 1);

        AddChild(enemyInstance);

        var movementComponent = enemyInstance.GetChild(0) as MovementComponent;
        movementComponent!.TargetPosition = Vector2.Zero;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _rand = new Random();

        // Loop through folder and add packed scenes
        const string rootPath = "res://scenes/characters/bugs/";

        using var dir = DirAccess.Open(rootPath);

        dir.ListDirBegin();
        var fileName = dir.GetNext();

        while (fileName != "")
        {
            _bugScenes.Add(GD.Load<PackedScene>(rootPath + fileName));

            fileName = dir.GetNext();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_enemiesSpawned == _enemiesPerWave)
            // Setup new wave
            return;

        if (_countdown <= 0)
        {
            SpawnEnemy(_bugScenes[_rand.Next(_bugScenes.Count)]);
            ++_enemiesSpawned;

            _countdown = 1.5;
        }

        _countdown -= delta;
    }
}