using Blossom.scripts.components;
using Blossom.scripts.controllers;
using Godot;

namespace Blossom.scripts;

public partial class BugSpawner : Node2D
{
    public static BugSpawner Instance;

    private const int Radius = 750;

    private Vector2 GetRandomPointAlongCircle()
    {
        var theta = GameController.Instance.Rand.NextDouble() * Mathf.Tau;

        return new Vector2((float)Mathf.Cos(theta), (float)Mathf.Sin(theta)) * Radius;
    }

    public void SpawnBug(PackedScene toSpawn)
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
        Instance ??= this;
    }
}