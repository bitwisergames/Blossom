using Godot;
using System;

[GlobalClass]
public partial class ShopCardInfo : Resource
{
    [Export] public Texture2D Texture;
    [Export] public string Name;
    [Export] public string Description;
    [Export] public int Cost;
    [Export] public PackedScene ToSpawn;
}