using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Blossom.scripts.controllers;

public partial class ShopController : CanvasLayer
{
    public static ShopController Instance;

    private List<ShopCardInfo> _possibleCards;

    private bool _lateInit;
    private int _refreshCount = -1;

    private Node shopContainer;

    public int CardsBought = 0;

    private int FindCostFromRefreshCount()
    {
        return Mathf.CeilToInt(Mathf.Pow(_refreshCount, 3) / 3);
    }

    private List<ShopCardInfo> GetCardsFromFolder(string rootPath)
    {
        List<ShopCardInfo> toReturn = [];

        using var dir = DirAccess.Open(rootPath);

        dir.ListDirBegin();
        var fileName = dir.GetNext();

        while (fileName != "")
        {
            toReturn.Add(GD.Load<ShopCardInfo>(rootPath + fileName));

            fileName = dir.GetNext();
        }

        return toReturn;
    }

    private void SetCard(int shopIndex, ShopCardInfo card)
    {
        var shopContainer = GetChild(0).GetChild(0).GetChild(0);

        var item = shopContainer.GetChildren().ToList()[shopIndex] as ShopItemUIController;

        item!.CardInfo = card;
        item.DisplayCard();
    }

    private void ShuffleCards()
    {
        if (GameController.Instance.SpendPollen(FindCostFromRefreshCount()))
        {
            for (var i = 0; i < 3; ++i)
            {
                if (i < 3 - CardsBought)
                {
                    SetCard(i, _possibleCards[GameController.Instance.Rand.Next(_possibleCards.Count)]);
                }
                else
                {
                    shopContainer.GetChildren().OfType<ShopItemUIController>().ToList()[i].ClearCard();
                }
            }

            ++_refreshCount;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance ??= this;

        shopContainer = GetChild(0).GetChild(0).GetChild(0);

        _possibleCards = GetCardsFromFolder("res://assets/resources/ShopCards/");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_lateInit) return;

        ShuffleCards();

        _lateInit = true;
    }
}