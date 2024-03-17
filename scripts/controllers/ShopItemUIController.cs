using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Blossom.scripts.controllers;

public partial class ShopItemUIController : MarginContainer
{
    private Label _title;
    private TextureRect _texture;
    private Label _cost;
    private Label _description;

    [Export] public ShopCardInfo cardInfo;

    public override void _Ready()
    {
        var container = GetChild(0).GetChild(0);
        _title = container.GetChildren().OfType<Label>().ToList()[0];
        _texture = container.GetChildren().OfType<TextureRect>().First();
        _cost = container.GetChildren().OfType<Label>().ToList()[1];
        _description = container.GetChildren().OfType<Label>().ToList()[2];

        _title.Text = cardInfo.Name;
        _texture.Texture = cardInfo.Texture;
        _cost.Text = "Cost: " + cardInfo.Cost + " pollen";
        _description.Text = cardInfo.Description;
    }

    public void OnButtonUp()
    {
        GameController.Instance.SetPlantScene(cardInfo.ToSpawn);
    }
}