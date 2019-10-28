using System;
using System.Drawing;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.Game.Objects.Buildings;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.Drawing;
using SadConsole;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CodeMagic.UI.Sad.Views.BuildingUI
{
    public class FireplaceUIView : BuildingUIView
    {
        private readonly Fireplace fireplace;
        private StandardButton addFuelButton;
        private StandardButton removeFuelButton;

        public FireplaceUIView(GameCore<Player> game, Fireplace fireplace) : base(game, "Fireplace")
        {
            this.fireplace = fireplace;

            InitializeControls();

            RefreshItems(false);
        }

        private void InitializeControls()
        {
            addFuelButton = new StandardButton(20)
            {
                Position = new Point(Width - 25, 6),
                Text = "Add Fuel"
            };
            addFuelButton.Click += (sender, args) => AddFuel();
            Add(addFuelButton);

            removeFuelButton = new StandardButton(20)
            {
                Position = new Point(Width - 25, 9),
                Text = "Remove Fuel"
            };
            removeFuelButton.Click += (sender, args) => RemoveFuel();
            Add(removeFuelButton);
        }

        private void AddFuel()
        {
            if (!(SelectedStack?.TopItem is IFuelItem fuel))
                return;

            if (fireplace.Fuel.Length >= fireplace.MaxFuelCount)
                return;

            Game.Player.Inventory.RemoveItem(fuel);
            fireplace.AddFuel(fuel);
            RefreshItems(true);
        }

        private void RemoveFuel()
        {
            if (fireplace.Fuel.Length == 0)
                return;

            var fuel = fireplace.RemoveLastFuel();
            Game.Player.Inventory.AddItem(fuel);
            RefreshItems(true);
        }

        protected override void ProcessSelectedItemChanged()
        {
            base.ProcessSelectedItemChanged();

            addFuelButton.IsVisible = SelectedStack?.TopItem is IFuelItem;
        }

        public override void Update(TimeSpan time)
        {
            Clear(new Rectangle(1, 3, 30, 20));

            base.Update(time);

            DrawProgress();
            DrawFuel();

            removeFuelButton.IsVisible = fireplace.Fuel.Length > 0;
        }

        private void DrawProgress()
        {
            if (fireplace.Fuel.Length == 0)
                return;

            const int progressX = 2;
            const int progressY = 5;
            const int progressLength = 20;

            var fuel = fireplace.Fuel.First();
            DrawLine(new Point(progressX, progressY), new Point(progressX + progressLength, progressY), background: Color.Red, glyph: ' ');
            var progress = (int) Math.Floor(progressLength * (fuel.FuelLeft / (double) fuel.MaxFuel));
            DrawLine(new Point(progressX, progressY), new Point(progressX + progress, progressY), background: Color.Lime, glyph: ' ');
        }

        private void DrawFuel()
        {
            Print(2, 3, "Fuel:");

            const int maxXPoint = 30;
            const int imagesX = 3;
            const int imagesY = 7;

            var x = imagesX;
            var y = imagesY;
            var maxHeight = 0;

            for (var index = 0; index < fireplace.MaxFuelCount; index++)
            {
                var fuel = index < fireplace.Fuel.Length ? fireplace.Fuel[index] : null;
                var image = (fuel as IInventoryImageProvider)?.GetInventoryImage(ImagesStorage.Current);

                var size = DrawImage(x, y, image);
                maxHeight = Math.Max(maxHeight, size.Height);
                
                x += size.Width + 3;
                if (x >= maxXPoint)
                {
                    x = imagesX;
                    y += maxHeight + 3;
                    maxHeight = 0;
                }
            }
        }

        private Size DrawImage(int x, int y, SymbolsImage image)
        {
            if (image != null)
            {
                DrawImage(x + 1, y + 1, image, DefaultForeground, DefaultBackground);
            }

            var width = image?.Width ?? 3;
            var height = image?.Height ?? 3;

            Print(x, y, new ColoredGlyph(Glyphs.GetGlyph('┌'), FrameColor, DefaultBackground));
            Print(x + width + 1, y, new ColoredGlyph(Glyphs.GetGlyph('┐'), FrameColor, DefaultBackground));
            Print(x, y + height + 1, new ColoredGlyph(Glyphs.GetGlyph('└'), FrameColor, DefaultBackground));
            Print(x + width + 1, y + height + 1, new ColoredGlyph(Glyphs.GetGlyph('┘'), FrameColor, DefaultBackground));

            return new Size(width, height);
        }
    }
}