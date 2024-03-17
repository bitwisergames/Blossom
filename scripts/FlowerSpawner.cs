using System;
using Blossom.scripts.controllers;
using Godot;

namespace Blossom.scripts;

public partial class FlowerSpawner : Node2D
{
    public static FlowerSpawner Instance;

    public void PlantFlower(PackedScene toSpawn, Vector2 pos, Action onPlantCallback)
    {
        // Will eventually be variable as to which enemy is being spawned
        var flowerInstance = toSpawn.Instantiate() as Node2D;

        flowerInstance!.GlobalPosition = pos;

        AddChild(flowerInstance);

        onPlantCallback.Invoke();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance ??= this;
    }
}