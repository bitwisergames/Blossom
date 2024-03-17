using System;
using System.Collections.Generic;
using System.Linq;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public partial class GameController : Node2D
{
    private Timer _spawnTimer;

    private int _enemiesAllowance;
    private int _enemiesSpawnedCost;

    private List<PackedScene> _bugScenes = [];

    private PackedScene _toSpawn;

    private Action _onPlantCallback;

    public static GameController Instance;
    public Random Rand;
    public bool WaveReadyToStart { get; private set; } = true;

    public int Pollen { get; private set; }
    public int WaveNumber = 1;

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

    private int EnemyAllowanceByLevel() => Mathf.FloorToInt((Math.Pow(WaveNumber, 2) / 3) + 10);

    private float TimeBetweenSpawnsByLevel() => 20f / (WaveNumber + 9f);

    private void SpawnBug()
    {
        BugSpawner.Instance.SpawnBug(_bugScenes[Rand.Next(_bugScenes.Count)]);
        ++_enemiesSpawnedCost;

        if (_enemiesSpawnedCost >= _enemiesAllowance)
        {
            _spawnTimer.Stop();
            _spawnTimer.QueueFree();
        }
    }

    private void HandleFlowerPlanting()
    {
        if (Input.IsActionJustPressed("Main"))
        {
            FlowerSpawner.Instance.PlantFlower(
                _toSpawn,
                GetGlobalMousePosition(),
                _onPlantCallback
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

    public bool SpendPollen(int amount)
    {
        if (amount > Pollen) return false;

        Pollen -= amount;
        return true;
    }

    public void StartWave()
    {
        if (!WaveReadyToStart) return;

        WaveReadyToStart = false;

        _enemiesAllowance = EnemyAllowanceByLevel();
        _enemiesSpawnedCost = 0;

        StartSpawningEnemies();
    }

    public void SetPlantScene(PackedScene plantScene, Action onPlantCallback)
    {
        _toSpawn = plantScene;
        _onPlantCallback = onPlantCallback;
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

        var livingBugs = BugSpawner.Instance.GetChildren().OfType<IDamagable>().Count();

        if (WaveReadyToStart || livingBugs > 0) return;

        HiveController.Instance.SetReady();
        WaveReadyToStart = true;
        ++WaveNumber;
        ShopController.Instance.ShuffleCards(true);
    }
}