using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.Game.Objects.Buildings;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Sad.Views.BuildingUI
{
    public class FurnaceUIView : BuildingUIView
    {
        private readonly Furnace furnace;
        private StandardButton putInputButton;
        private StandardButton removeInputButton;
        private StandardButton removeOutputButton;

        public FurnaceUIView(IGameCore game, Furnace furnace) : base(game, "Furnace")
        {
            this.furnace = furnace;

            InitializeControls();

            RefreshItems(false);
        }

        private void InitializeControls()
        {
            putInputButton = new StandardButton(20)
            {
                Position = new Point(Width - 25, 6),
                Text = "Put on Input"
            };
            putInputButton.Click += (sender, args) => PutItemOnInput();
            Add(putInputButton);

            removeInputButton = new StandardButton(20)
            {
                Position = new Point(Width - 25, 9),
                Text = "Remove Input"
            };
            removeInputButton.Click += (sender, args) => RemoveItemFromInput();
            Add(removeInputButton);

            removeOutputButton = new StandardButton(20)
            {
                Position = new Point(Width - 25, 12),
                Text = "Remove Output"
            };
            removeOutputButton.Click += (sender, args) => RemoveItemFromOutput();
            Add(removeOutputButton);
        }

        private void RemoveItemFromOutput()
        {
            if (furnace.OutputItem == null)
                return;

            Game.Player.Inventory.AddItem(furnace.OutputItem);
            furnace.OutputItem = null;

            RefreshItems(true);
        }

        private void RemoveItemFromInput()
        {
            if (furnace.InputItem == null)
                return;

            Game.Player.Inventory.AddItem(furnace.InputItem);
            furnace.InputItem = null;

            RefreshItems(true);
        }

        private void PutItemOnInput()
        {
            var item = SelectedStack?.TopItem as IFurnaceItem;
            if (item == null)
                return;

            RemoveItemFromInput();

            Game.Player.Inventory.RemoveItem(item);
            furnace.InputItem = item;

            RefreshItems(true);
        }

        public override void Update(TimeSpan time)
        {
            Clear(new Rectangle(2, 2, 30, 22));

            base.Update(time);

            Print(2, 5, "Input");
            Print(20, 5, "Output");

            DrawInputItem();
            DrawOutputItem();
            DrawProgress();

            putInputButton.IsVisible = (SelectedStack?.TopItem as IFurnaceItem) != null;
            removeInputButton.IsVisible = furnace.InputItem != null;
            removeOutputButton.IsVisible = furnace.OutputItem != null;
        }

        private void DrawProgress()
        {
            if (furnace.InputItem == null)
                return;

            var progressPercent = furnace.CurrentProgress / (double)furnace.InputItem.FurnaceProcessingTime;

            const int startX = 2;
            const int startY = 22;
            const int length = 25;
            
            DrawLine(new Point(startX, startY), new Point(startX + length, startY), DefaultForeground, Color.Gray, ' ');
            var progressLineLength = (int) Math.Floor(progressPercent * length);
            if (progressLineLength > 0)
            {
                DrawLine(new Point(startX, startY), new Point(startX + progressLineLength, startY), DefaultForeground, Color.Red, ' ');
            }
        }

        private void DrawInputItem()
        {
            const int xShift = 2;
            const int yShift = 6;

            var inputItemImage = (furnace.InputItem as IInventoryImageProvider)?.GetInventoryImage(ImagesStorage.Current);
            var inputItemWidth = inputItemImage?.Width ?? 3;
            var inputItemHeight = inputItemImage?.Height ?? 3;

            if (inputItemImage != null)
            {
                DrawImage(xShift + 1, yShift + 1, inputItemImage, DefaultForeground, DefaultBackground);
            }

            Print(xShift, yShift, new ColoredGlyph(Glyphs.GetGlyph('┌'), FrameColor, DefaultBackground));
            Print(xShift + inputItemWidth + 1, yShift, new ColoredGlyph(Glyphs.GetGlyph('┐'), FrameColor, DefaultBackground));
            Print(xShift, yShift + inputItemHeight + 1, new ColoredGlyph(Glyphs.GetGlyph('└'), FrameColor, DefaultBackground));
            Print(xShift + inputItemWidth + 1, yShift + inputItemHeight + 1, new ColoredGlyph(Glyphs.GetGlyph('┘'), FrameColor, DefaultBackground));
        }

        private void DrawOutputItem()
        {
            const int xShift = 20;
            const int yShift = 6;

            var outputItemImage = (furnace.OutputItem as IInventoryImageProvider)?.GetInventoryImage(ImagesStorage.Current);
            var outputItemWidth = outputItemImage?.Width ?? 3;
            var outputItemHeight = outputItemImage?.Height ?? 3;

            if (outputItemImage != null)
            {
                DrawImage(xShift + 1, yShift + 1, outputItemImage, DefaultForeground, DefaultBackground);
            }

            Print(xShift, yShift, new ColoredGlyph(Glyphs.GetGlyph('┌'), FrameColor, DefaultBackground));
            Print(xShift + outputItemWidth + 1, yShift, new ColoredGlyph(Glyphs.GetGlyph('┐'), FrameColor, DefaultBackground));
            Print(xShift, yShift + outputItemHeight + 1, new ColoredGlyph(Glyphs.GetGlyph('└'), FrameColor, DefaultBackground));
            Print(xShift + outputItemWidth + 1, yShift + outputItemHeight + 1, new ColoredGlyph(Glyphs.GetGlyph('┘'), FrameColor, DefaultBackground));

        }
    }
}