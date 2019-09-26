using System;
using CodeMagic.Configuration.Xml.Types.Buildings;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class BuildingDetailsControl : ControlBase
    {
        private static readonly Color DescriptionColor = Color.Gray;
        private static readonly Color FrameColor = Color.Gray;

        public BuildingDetailsControl(int width, int height)
            : base(width, height)
        {
            Theme = new DrawingSurfaceTheme();
            CanFocus = false;
        }

        public XmlBuildingConfiguration Building { get; set; }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Surface.Print(1, 0, "Selected Building Details");
            Surface.Fill(0, 1, Width, FrameColor, null, Glyphs.GetGlyph('─'));

            Surface.Fill(1, 3, 15, Color.Black, Color.Black, null);
            if (Building == null)
            {
                Surface.Print(1, 3, new ColoredString("Building is not selected", new Cell(Color.Gray, Color.Black)));
            }
            else
            {
                var image = ImagesStorage.Current.GetImage(Building.PreviewImage);
                Surface.DrawImage(2, 3, image, Color.White, Color.Black);

                var nameColor = ItemDrawingHelper.GetRarenessColor(Building.Rareness);
                Surface.Print(1, 4 + image.Height, Building.Name, nameColor);

                for (var lineIndex = 0; lineIndex < Building.Description.Length; lineIndex++)
                {
                    var line = Building.Description[lineIndex];
                    Surface.Print(1, 6 + image.Height + lineIndex, line, DescriptionColor);
                }
            }
        }
    }
}