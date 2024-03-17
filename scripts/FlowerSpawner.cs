using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Blossom.scripts;

public partial class FlowerSpawner : Node2D
{
    private List<PackedScene> _flowerScenes = new();
    private Random _rand;

    private void PlantFlower(PackedScene toSpawn, Vector2 pos)
    {
        // Will eventually be variable as to which enemy is being spawned
        var flowerInstance = toSpawn.Instantiate() as Node2D;

        flowerInstance!.GlobalPosition = pos;

        AddChild(flowerInstance);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _rand = new Random();

        // Loop through folder and add packed scenes
        const string rootPath = "res://scenes/characters/flowers/";

        using var dir = DirAccess.Open(rootPath);

        dir.ListDirBegin();
        var fileName = dir.GetNext();

        while (fileName != "")
        {
            _flowerScenes.Add(GD.Load<PackedScene>(rootPath + fileName));

            fileName = dir.GetNext();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Main"))
        {
            PlantFlower(
                _flowerScenes[1],
                GetGlobalMousePosition()
            );
        }
    }
}