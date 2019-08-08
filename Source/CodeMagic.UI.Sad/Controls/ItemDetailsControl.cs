using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Items;
using CodeMagic.Implementations.Items;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using CodeMagic.UI.Sad.Drawing.ImageProviding;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class ItemDetailsControl : ControlBase
    {
        private static readonly Color FrameColor = Color.Gray;
        private static readonly Color BackColor = Color.Black;

        private readonly ImagesFactory imagesFactory;

        public ItemDetailsControl(int width, int height) 
            : base(width, height)
        {
            Theme = new DrawingSurfaceTheme();
            CanFocus = false;

            imagesFactory = new ImagesFactory(ImagesStorage.Current);
        }

        public InventoryStack Stack { get; set; }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Surface.Clear();

            Surface.Print(1, 0, "Selected Item Details");
            Surface.Fill(0, 1, Width, FrameColor, null, Glyphs.GlyphBoxSingleHorizontal);

            Surface.Fill(1, 3, 15, BackColor, BackColor, null);

            if (Stack == null)
                return;

            DrawName();

            var itemImage = imagesFactory.GetImage(Stack.Item);
            if (itemImage != null)
            {
                Surface.DrawImage(3, 6, itemImage, Color.White, BackColor);
            }

            if (Stack.Item is IDescriptionProvider descriptionProvider)
            {
                var imageHeight = itemImage?.Height ?? 0;
                var descriptionY = 6 + 1 + imageHeight;
                DrawDescription(descriptionY, descriptionProvider);
            }
        }

        private void DrawName()
        {
            const int initialYShift = 3;
            var maxWidth = Width - 6;

            var itemColor = ItemDrawingHelper.GetItemColor(Stack.Item);
            if (Stack.Item.Name.Length <= maxWidth)
            {
                Surface.Print(1, initialYShift, new ColoredString(Stack.Item.Name, new Cell(itemColor, BackColor)));
                return;
            }

            var cuttedName = CutNameString(Stack.Item.Name, maxWidth);
            for (int yShift = 0; yShift < cuttedName.Length; yShift++)
            {
                var line = cuttedName[yShift];
                Surface.Print(1, initialYShift + yShift, new ColoredString(line, new Cell(itemColor, BackColor)));
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

        private void DrawDescription(int initialY, IDescriptionProvider descriptionProvider)
        {
            const int initialX = 3;
            var lines = descriptionProvider.GetDescription();
            for (int yShift = 0; yShift < lines.Length; yShift++)
            {
                var line = lines[yShift].Select(part =>
                    new ColoredString(part.String, ColorHelper.ConvertToXna(part.TextColor), BackColor)).ToArray();
                var y = initialY + yShift;
                Surface.PrintStyledText(initialX, y, line);
            }
        }
    }
}