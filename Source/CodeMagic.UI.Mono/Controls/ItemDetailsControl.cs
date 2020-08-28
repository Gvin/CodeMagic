using System.Collections.Generic;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Drawing.ImageProviding;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using CodeMagic.UI.Mono.Views;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Controls
{
    public class ItemDetailsControl : Control
    {
        private readonly InventoryImagesFactory imagesFactory;
        private readonly Player player;

        public ItemDetailsControl(Rectangle location, Player player)
            : base(location)
        {
            this.player = player;

            imagesFactory = new InventoryImagesFactory(ImagesStorage.Current);
        }

        public override void Draw(ICellSurface surface)
        {
            surface.Write(1, 0, "Selected Item Details");
            surface.Fill(new Rectangle(0, 1, Location.Width, 1), new Cell('─', BaseWindow.FrameColor));

            if (Stack == null)
                return;

            DrawName(surface);

            var itemImage = imagesFactory.GetImage(Stack.TopItem);
            if (itemImage != null)
            {
                surface.DrawImage(3, 6, itemImage, Color.White, Color.Black);
            }

            if (Stack.TopItem is IDescriptionProvider descriptionProvider)
            {
                var imageHeight = itemImage?.Height ?? 0;
                var descriptionY = 6 + 1 + imageHeight;
                DrawDescription(surface, descriptionY, descriptionProvider);
            }
        }

        public InventoryStack Stack { get; set; }

        private void DrawName(ICellSurface surface)
        {
            const int initialYShift = 3;
            var maxWidth = Location.Width - 6;

            var itemColor = ItemDrawingHelper.GetItemColor(Stack.TopItem).ToXna();
            if (Stack.TopItem.Name.Length <= maxWidth)
            {
                surface.Write(1, initialYShift, new ColoredString(Stack.TopItem.Name, itemColor));
                return;
            }

            var cuttedName = CutNameString(Stack.TopItem.Name, maxWidth);
            for (int yShift = 0; yShift < cuttedName.Length; yShift++)
            {
                var line = cuttedName[yShift];
                surface.Write(1, initialYShift + yShift, new ColoredString(line, itemColor));
            }
        }

        private string[] CutNameString(string name, int maxWidth)
        {
            var result = new List<string>();
            var remainingName = name;

            while (remainingName.Length > maxWidth)
            {
                var cutIndex = remainingName.Substring(0, maxWidth).LastIndexOf(' ');
                var leftPart = remainingName.Substring(0, cutIndex);
                var rightPart = remainingName.Substring(cutIndex + 1, remainingName.Length - cutIndex - 1);

                result.Add(leftPart);
                remainingName = rightPart;
            }

            result.Add(remainingName);

            return result.ToArray();
        }

        private void DrawDescription(ICellSurface surface, int initialY, IDescriptionProvider descriptionProvider)
        {
            const int initialX = 1;
            var descriptionLines = TextFormatHelper.SplitText(descriptionProvider.GetDescription(player),
                Location.Width - initialX - 1, Color.Black);

            for (int yShift = 0; yShift < descriptionLines.Length; yShift++)
            {
                var line = descriptionLines[yShift];
                var y = initialY + yShift;
                surface.Write(initialX, y, line);
            }
        }
    }
}