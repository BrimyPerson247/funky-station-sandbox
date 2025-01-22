﻿using Content.Client.Stylesheets;
using Content.Shared._Funkystation.Medical.SmartFridge;
using Content.Shared.FixedPoint;
using Robust.Client.AutoGenerated;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;

namespace Content.Client._Funkystation.Medical.SmartFridge.UI;

[GenerateTypedNameReferences]
public sealed partial class SmartFridgeItem : PanelContainer
{
    public event Action<BaseButton.ButtonEventArgs, DispenseButton>? OnItemSelected;
    public SmartFridgeItem(EntProtoId entProto, string name, FixedPoint2 quantity)
    {
        RobustXamlLoader.Load(this);

        ItemPrototype.SetPrototype(entProto);
        NameLabel.Text = name;
        NameQuantity.Text = $"{quantity}";
    }

    public DispenseButton GetDispenseButtons(int index, string name)
    {
        var dispenseButtonConstructor = MakeDispenseButton(index, name);

        PrimaryContainer.AddChild(dispenseButtonConstructor);

        SetBackgroundColor(index);

        return dispenseButtonConstructor;
    }

    /// <summary>
    /// sets alternating background colors for each row, based off of the item's index
    /// </summary>
    private void SetBackgroundColor(int index)
    {
        var rowColor1 = Color.FromHex("#202025");
        var rowColor2 = Color.FromHex("#1B1B1E");
        var currentRowColor = (index % 2 == 1) ? rowColor1 : rowColor2;

        ParentContainer.PanelOverride = new StyleBoxFlat(currentRowColor);
    }

    private DispenseButton MakeDispenseButton(int index, string itemName)
    {
        var text = Loc.GetString("smart-fridge-label-dispense");

        var button = new DispenseButton(text, index, itemName);

        button.OnPressed += args
            => OnItemSelected?.Invoke(args, button);

        return button;
    }

    public sealed class DispenseButton : Button
    {
        public int Index { get; set; }
        public string ItemName { get; set; }
        public DispenseButton(string text, int index, string itemName)
        {
            Text = text;
            Index = index;
            ItemName = itemName;
        }
    }
}
