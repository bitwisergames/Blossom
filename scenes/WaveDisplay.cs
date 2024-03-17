using Godot;
using Blossom.scripts.controllers;

public partial class WaveDisplay : Label
{
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Text = $"Wave: {GameController.Instance.WaveNumber}";
    }
}