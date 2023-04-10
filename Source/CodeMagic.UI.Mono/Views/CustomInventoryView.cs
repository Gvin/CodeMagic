using System;
using CodeMagic.Core.Items;
using CodeMagic.UI.Mono.Drawing.ImageProviding;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Presenters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views;

public class CustomInventoryView : InventoryViewBase, ICustomInventoryView
{
    private FramedButton _pickUpStackButton;
    private FramedButton _pickUpOneItemButton;
    private FramedButton _pickUpAllButton;
    private string _inventoryName;

    public event EventHandler PickUpOne;
    public event EventHandler PickUpStack;
    public event EventHandler PickUpAll;

    public CustomInventoryView(IInventoryImagesFactory inventoryImagesFactory)
        : base(inventoryImagesFactory)
    {
    }

    string ICustomInventoryView.InventoryName
    {
        set => _inventoryName = value;
    }

    protected override string InventoryName => _inventoryName;

    public override void Initialize()
    {
        base.Initialize();
        InitializeControls();

        RefreshItems(false);
    }

    void ICustomInventoryView.RefreshItems(bool keepSelection)
    {
        RefreshItems(keepSelection);
    }

    private void InitializeControls()
    {
        _pickUpOneItemButton = new FramedButton(new Rectangle(Width - 52, 40, 20, 3))
        {
            Text = "[O] Pick Up One"
        };
        _pickUpOneItemButton.Click += (_, _) => PickUpOne?.Invoke(this, EventArgs.Empty);
        Controls.Add(_pickUpOneItemButton);

        _pickUpStackButton = new FramedButton(new Rectangle(Width - 52, 43, 20, 3))
        {
            Text = "[P] Pick Up"
        };
        _pickUpStackButton.Click += (_, _) => PickUpStack?.Invoke(this, EventArgs.Empty);
        Controls.Add(_pickUpStackButton);

        _pickUpAllButton = new FramedButton(new Rectangle(Width - 52, 46, 20, 3))
        {
            Text = "[A] Pick Up All"
        };
        _pickUpAllButton.Click += (_, _) => PickUpAll?.Invoke(this, EventArgs.Empty);
        Controls.Add(_pickUpAllButton);
    }

    public override void Update(TimeSpan time)
    {
        base.Update(time);

        UpdateButtonsState();
    }

    private void UpdateButtonsState()
    {
        _pickUpOneItemButton.Visible = SelectedStack?.TopItem != null && SelectedStack.TopItem.Stackable;
        _pickUpStackButton.Visible = SelectedStack != null;
    }

    public override bool ProcessKeysPressed(Keys[] keys)
    {
        if (keys.Length == 1)
        {
            switch (keys[0])
            {
                case Keys.A:
                    PickUpAll?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.P:
                    PickUpStack?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.O:
                    PickUpOne?.Invoke(this, EventArgs.Empty);
                    return true;
            }
        }

        return base.ProcessKeysPressed(keys);
    }

    protected override InventoryStackItem CreateListBoxItem(InventoryStack stack)
    {
        return new CustomInventoryItem(stack);
    }
}

public class CustomInventoryItem : InventoryStackItem
{
    public CustomInventoryItem(InventoryStack itemStack)
        : base(itemStack)
    {
    }

    protected override ColoredString[] GetAfterNameText(Color backColor)
    {
        if (Stack.TopItem.Stackable)
        {
            return new[]
            {
                new ColoredString($" ({Stack.Count})", StackCountColor, backColor)
            };
        }

        return Array.Empty<ColoredString>();
    }
}
