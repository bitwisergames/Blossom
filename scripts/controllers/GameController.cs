using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Blossom.scripts.controllers;

public partial class GameController : Node2D
{
    private Timer _spawnTimer;

    private int _enemiesAllowance;
    private int _enemiesSpawnedCost;
    private int _waveNumber = 1;

    private List<PackedScene> _bugScenes = [];

    private PackedScene _toSpawn;

    public static GameController Instance;
    public Random Rand;
    public bool WaveReadyToStart { get; private set; } = true;

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

    private int EnemyAllowanceByLevel() => Mathf.FloorToInt((Math.Pow(_waveNumber, 2) / 3) + 10);

    private float TimeBetweenSpawnsByLevel() => 20f / (_waveNumber + 9f);

    private void SpawnBug()
    {
        BugSpawner.Instance.SpawnBug(_bugScenes[Rand.Next(_bugScenes.Count)]);
        ++_enemiesSpawnedCost;

        if (_enemiesSpawnedCost >= _enemiesAllowance)
        {
            ++_waveNumber;
            _spawnTimer.Stop();
            _spawnTimer.QueueFree();

            HiveController.Instance.SetReady();
            WaveReadyToStart = true;
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

    private void StartSpawningEnemies()
    {
        SpawnBug();

        _spawnTimer = new Timer();
        _spawnTimer.WaitTime = TimeBetweenSpawnsByLevel();
        _spawnTimer.OneShot = false;
        _spawnTimer.Timeout += SpawnBug;

        AddChild(_spawnTimer);
        _spawnTimer.Start();
    }

    public void StartWave()
    {
        if (!WaveReadyToStart) return;

        WaveReadyToStart = false;

        _enemiesAllowance = EnemyAllowanceByLevel();
        _enemiesSpawnedCost = 0;

        StartSpawningEnemies();
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