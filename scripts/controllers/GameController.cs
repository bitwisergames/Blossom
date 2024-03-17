using System;
using System.Collections.Generic;
using System.Linq;
using Blossom.scripts.controllers.flowers;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public partial class GameController : Node2D
{
    private Timer _spawnTimer;

    private int _enemiesAllowance;
    private int _enemiesSpawnedCost;

    [Export] private BugSpawnInfo[] _bugScenes;

    private PackedScene _toSpawn;

    private Action _onPlantCallback;

    public static GameController Instance;
    public Random Rand;
    public bool WaveReadyToStart { get; private set; } = true;

    public int NumOfBees = 1;
    public int BeeLevel = 1;

    public int Pollen { get; private set; }
    public int WaveNumber = 1;

    private int EnemyAllowanceByLevel() => Mathf.FloorToInt((Math.Pow(WaveNumber, 2) / 3) + 10);

    private float TimeBetweenSpawnsByLevel() => 20f / (WaveNumber + 9f);

    private void SpawnBug()
    {
        BugSpawnInfo info;
        do info = _bugScenes[Rand.Next(_bugScenes.Length)];
        while (WaveNumber < info.MinimumWave);

        BugSpawner.Instance.SpawnBug(info.ToSpawn);
        _enemiesSpawnedCost += info.AntEquivalent;

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

    public void AddPollen(int amount)
    {
        if (amount < 0) return;

        Pollen += amount;
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

        var level = GameController.Instance.GetNode<Node2D>("Level");
        var flowerController = level.GetNode<Node2D>("FlowerController");
        var options = flowerController.GetChildren();

        foreach (var option in options)
        {
            var flower = option as BaseFlowerController;
            flower?.ResetPollination();

            GD.Print("Resetting");
        }
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