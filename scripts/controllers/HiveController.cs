using Godot;

namespace Blossom.scripts.controllers;

public partial class HiveController : Sprite2D
{
    public int Health { get; private set; } = 100;

    public static HiveController Instance { get; private set; }

    public void InflictDamage(int amount)
    {
        Health -= amount;

        if (Health <= 0) GD.Print("ded");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance ??= this;
    }
}