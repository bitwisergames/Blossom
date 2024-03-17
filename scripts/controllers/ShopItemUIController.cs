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

    [Export] public ShopCardInfo CardInfo;

    public void ClearCard()
    {
        _title.Text = "";
        _texture.Texture = null;
        _cost.Text = "";
        _description.Text = "";

        ++ShopController.Instance.CardsBought;
    }

    public void DisplayCard()
    {
        _title.Text = CardInfo.Name;
        _texture.Texture = CardInfo.Texture;
        _cost.Text = "Cost: " + CardInfo.Cost + " pollen";
        _description.Text = CardInfo.Description;
    }

    public override void _Ready()
    {
        var container = GetChild(0).GetChild(0);
        _title = container.GetChildren().OfType<Label>().ToList()[0];
        _texture = container.GetChildren().OfType<TextureRect>().First();
        _cost = container.GetChildren().OfType<Label>().ToList()[1];
        _description = container.GetChildren().OfType<Label>().ToList()[2];

        DisplayCard();
    }

    public void OnButtonUp()
    {
        GameController.Instance.SetPlantScene(CardInfo.ToSpawn, ClearCard);
    }
}