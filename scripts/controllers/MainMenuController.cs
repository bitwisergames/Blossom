using Godot;
using System;

public partial class MainMenuController : Control
{
    public void ChangeSceneToPlay()
    {
        GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://scenes/Play.tscn"));
    }

    public void Quit()
    {
        GetTree().Quit();
    }
}