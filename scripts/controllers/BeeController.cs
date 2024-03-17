using System.Linq;
using Blossom.scripts.controllers.flowers;
using Blossom.scripts.interfaces;
using Godot;

namespace Blossom.scripts.controllers;

public partial class BeeController : Node2D, IMoveable
{
    private CharacterBody2D _body;
    private BaseFlowerController _flowerController;

    public float Speed => 3500f;
    public bool Flying => true;
    public Vector2 TargetPosition { get; private set; }

    private void ReachedDestination()
    {
        if (TargetPosition != Vector2.Zero)
        {
            TargetPosition = Vector2.Zero;

            GameController.Instance.AddPollen(_flowerController.Income[_flowerController.Stage]);
            ShopController.Instance.UpdatePollenCounter();

            _flowerController.Pollinate();
        }
        else
        {
            QueueFree();
        }
    }

    public void FindFlowerToPollinate()
    {
        var level = GameController.Instance.GetNode<Node2D>("Level");
        var flowerController = level.GetNode<Node2D>("FlowerController");
        var options = flowerController.GetChildren();

        var selectedOption = (Node2D)options[GameController.Instance.Rand.Next(options.Count)];

        _flowerController = selectedOption as BaseFlowerController;

        TargetPosition = selectedOption.GlobalPosition;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _body = GetChild<CharacterBody2D>(0);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_body.GlobalPosition.DistanceSquaredTo(TargetPosition) <= 5f) ReachedDestination();

        var dir = _body.GlobalPosition.DirectionTo(TargetPosition);
        _body.Velocity = dir.Normalized() * Speed * (float)delta;

        _body.MoveAndSlide();
    }
}