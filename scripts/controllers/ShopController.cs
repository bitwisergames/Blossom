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

    private Node _shopContainer;
    private AnimationPlayer _animationPlayer;
    private Label _pollenLabel;
    private Button _refreshButton;
    private Button _buyBeeButton;
    private Button _upgradeBeeButton;

    private bool _isShopOpen;

    public int CardsBought = 0;

    private int FindCostOfRefresh()
    {
        return Mathf.CeilToInt(Mathf.Pow(_refreshCount, 3) / 3);
    }

    private int FindCostOfNewBee()
    {
        return Mathf.CeilToInt(Mathf.Pow(2, GameController.Instance.NumOfBees));
    }

    private int FindCostOfBeeUpgrade()
    {
        return Mathf.CeilToInt(Mathf.Pow(2, 2 * GameController.Instance.BeeLevel) + 6);
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

        card.isFree = GameController.Instance.WaveNumber == 1;

        item!.CardInfo = card;
        item.DisplayCard();
    }

    public void UpdatePollenCounter()
    {
        _pollenLabel.Text = GameController.Instance.Pollen + " Pollen";
    }

    public void UpdateAddBeeDisplay()
    {
        _buyBeeButton.Text = $"+1 Bee ({FindCostOfNewBee()})";
        UpdatePollenCounter();
    }

    public void UpdateBetterBeeDisplay()
    {
        _upgradeBeeButton.Text = $"Better Bees ({FindCostOfBeeUpgrade()})";
        UpdatePollenCounter();
    }

    public void UpdateRefreshDisplay()
    {
        _refreshButton.Text = "Refresh (" + FindCostOfRefresh() + ")";
        UpdatePollenCounter();
    }

    public void AddBee()
    {
        if (GameController.Instance.SpendPollen(FindCostOfNewBee()))
        {
            ++GameController.Instance.NumOfBees;
            UpdateAddBeeDisplay();
        }
    }

    public void UpgradeBees()
    {
        if (GameController.Instance.SpendPollen(FindCostOfBeeUpgrade()))
        {
            ++GameController.Instance.BeeLevel;

            UpdateBetterBeeDisplay();
        }
    }

    public void ShuffleCards()
    {
        ShuffleCards(false);
    }

    public void ShuffleCards(bool reset)
    {
        if (reset)
        {
            CardsBought = 0;
            _refreshCount = 0;
        }

        if (GameController.Instance.SpendPollen(FindCostOfRefresh()))
        {
            for (var i = 0; i < 3; ++i)
            {
                if (i < 3 - CardsBought)
                {
                    SetCard(i, _possibleCards[GameController.Instance.Rand.Next(_possibleCards.Count)]);
                }
                else
                {
                    _shopContainer.GetChildren().OfType<ShopItemUIController>().ToList()[i].ClearCard();
                }
            }

            if (!reset)
                ++_refreshCount;

            UpdateRefreshDisplay();
        }
    }

    public void SetIsOpen(bool isOpen)
    {
        _isShopOpen = isOpen;
    }

    private void LateLoad()
    {
        _pollenLabel = GetNode<Label>("Panel/Panel/Label");
        UpdatePollenCounter();

        _refreshButton = GetNode<Button>("Panel/MarginContainer/ShopItemContainer/VBoxContainer/Refresh Shop");
        UpdateRefreshDisplay();

        _buyBeeButton = GetNode<Button>("Panel/MarginContainer/ShopItemContainer/VBoxContainer/Buy Bee");
        UpdateAddBeeDisplay();

        _upgradeBeeButton = GetNode<Button>("Panel/MarginContainer/ShopItemContainer/VBoxContainer/Better Bees");
        UpdateBetterBeeDisplay();

        ShuffleCards(true);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion eventMouseMotion) return;
        if (!GameController.Instance.WaveReadyToStart) return;

        switch (_isShopOpen)
        {
            case false when eventMouseMotion.Position.Y >= 950:
                _animationPlayer.Play("ShowShop");
                break;
            case true when eventMouseMotion.Position.Y < 750:
                _animationPlayer.Play("HideShop");
                break;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance ??= this;

        _shopContainer = GetChild(0).GetChild(0).GetChild(0);
        _animationPlayer = GetChildren().OfType<AnimationPlayer>().First();
        _animationPlayer.Play("HideShop");

        _possibleCards = GetCardsFromFolder("res://assets/resources/ShopCards/");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_lateInit) return;

        LateLoad();
        _lateInit = true;
    }
}