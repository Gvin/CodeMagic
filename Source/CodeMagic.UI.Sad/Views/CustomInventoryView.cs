using System;
using CodeMagic.Core.Items;
using CodeMagic.UI.Presenters;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;
using SadConsole.Input;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views
{
    public class CustomInventoryView : InventoryViewBase, ICustomInventoryView
    {
        private StandardButton pickUpStackButton;
        private StandardButton pickUpOneItemButton;
        private StandardButton pickUpAllButton;
        private string inventoryName;

        public event EventHandler PickUpOne;
        public event EventHandler PickUpStack;
        public event EventHandler PickUpAll;

        string ICustomInventoryView.InventoryName
        {
            set => inventoryName = value;
        }

        protected override string InventoryName => inventoryName;

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
            pickUpOneItemButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 40),
                Text = "[O] Pick Up One"
            };
            pickUpOneItemButton.Click += (sender, args) => PickUpOne?.Invoke(this, EventArgs.Empty);
            Add(pickUpOneItemButton);

            pickUpStackButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 43),
                Text = "[P] Pick Up"
            };
            pickUpStackButton.Click += (sender, args) => PickUpStack?.Invoke(this, EventArgs.Empty);
            Add(pickUpStackButton);

            pickUpAllButton = new StandardButton(20)
            {
                Position = new Point(Width - 52, 46),
                Text = "[A] Pick Up All"
            };
            pickUpAllButton.Click += (sender, args) => PickUpAll?.Invoke(this, EventArgs.Empty);
            Add(pickUpAllButton);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            UpdateButtonsState();
        }

        private void UpdateButtonsState()
        {
            pickUpOneItemButton.IsVisible = SelectedStack?.TopItem != null && SelectedStack.TopItem.Stackable;
            pickUpStackButton.IsVisible = SelectedStack != null;
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
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

            return base.ProcessKeyPressed(key);
        }

        protected override InventoryStackItem CreateListBoxItem(InventoryStack stack)
        {
            return new CustomInventoryItem(stack);
        }

        public void Close()
        {
            Close(DialogResult.None);
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
                    new ColoredString($" ({Stack.Count})", new Cell(StackCountColor, backColor))
                };
            }

            return new ColoredString[0];
        }
    }
}