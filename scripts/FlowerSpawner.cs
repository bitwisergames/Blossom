using System.Linq;
using Blossom.scripts.controllers;
using Godot;

namespace Blossom.scripts;

public partial class FlowerSpawner : Node2D
{
    private void PlantFlower(FlowerController.FlowerInfo info, Vector2 pos)
    {
        var flowerScene = GD.Load<PackedScene>("res://scenes/Flower.tscn");

        // Will eventually be variable as to which enemy is being spawned
        var flowerInstance = flowerScene.Instantiate() as Node2D;

        flowerInstance!.GlobalPosition = pos;

        flowerInstance.GetChildren().OfType<Sprite2D>().First().Texture = GD.Load<Texture2D>(info.SpriteResPath);

        AddChild(flowerInstance);

        var flowerController = flowerInstance as FlowerController;
        flowerController!.Setup(info);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Main"))
        {
            PlantFlower(new FlowerController.FlowerInfo(
                    "res://assets/sprites/RedFlower.png",
                    3,
                    1.2f,
                    200,
                    false
                ),
                GetGlobalMousePosition()
            );
        }
    }
}