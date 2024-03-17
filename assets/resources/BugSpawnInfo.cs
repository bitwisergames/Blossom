using Godot;

[GlobalClass]
public partial class BugSpawnInfo : Resource
{
    [Export] public int MinimumWave;
    [Export] public int AntEquivalent;
    [Export] public PackedScene ToSpawn;
}