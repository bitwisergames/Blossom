using System;
using System.Collections.Generic;
using Godot;

namespace Blossom.scripts.controllers;

public partial class GameController : Node2D
{
    private Timer _spawnTimer;
    private Timer _waveTimer;

    private int _enemiesAllowance;
    private int _enemiesSpawnedCost = 0;

    private List<PackedScene> _bugScenes = [];

    private PackedScene _toSpawn;

    public static GameController Instance;
    public Random Rand;

    private List<PackedScene> GetScenesFromFolder(string rootPath)
    {
        List<PackedScene> toReturn = [];

        using var dir = DirAccess.Open(rootPath);

        dir.ListDirBegin();
        var fileName = dir.GetNext();

        while (fileName != "")
        {
            toReturn.Add(GD.Load<PackedScene>(rootPath + fileName));

            fileName = dir.GetNext();
        }

        return toReturn;
    }

    private void SpawnBug()
    {
        BugSpawner.Instance.SpawnBug(_bugScenes[Rand.Next(_bugScenes.Count)]);
        ++_enemiesSpawnedCost;

        if (_enemiesSpawnedCost >= _enemiesAllowance)
        {
            _spawnTimer.Stop();
            _spawnTimer.QueueFree();
            StartWave(10);
        }
    }

    private void HandleFlowerPlanting()
    {
        if (Input.IsActionJustPressed("Main"))
        {
            FlowerSpawner.Instance.PlantFlower(
                _toSpawn,
                GetGlobalMousePosition()
            );

            _toSpawn = null;
        }
    }

    private void StartWave(int antEquivalent)
    {
        _enemiesAllowance = antEquivalent;
        _enemiesSpawnedCost = 0;

        _waveTimer = new Timer();
        _waveTimer.WaitTime = 20;
        _waveTimer.OneShot = true;
        _waveTimer.Timeout += StartSpawningEnemies;

        AddChild(_waveTimer);
        _waveTimer.Start();
    }

    private void StartSpawningEnemies()
    {
        SpawnBug();

        _spawnTimer = new Timer();
        _spawnTimer.WaitTime = 2;
        _spawnTimer.OneShot = false;
        _spawnTimer.Timeout += SpawnBug;

        AddChild(_spawnTimer);
        _spawnTimer.Start();
    }

    public void SetPlantScene(PackedScene plantScene)
    {
        _toSpawn = plantScene;
    }

    public override void _Ready()
    {
        Instance ??= this;

        Rand = new Random();

        _bugScenes = GetScenesFromFolder("res://scenes/characters/bugs/");

        StartWave(10);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_toSpawn != null)
        {
            HandleFlowerPlanting();
        }
    }
}