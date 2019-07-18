using System.Drawing;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects.Creatures.Implementations;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation.Monsters
{
    public class GoblinDrawingProcessor : IDrawingProcessor
    {
        private static readonly Color FaceColor = Color.Red;
        private static readonly Color BodyColor = Color.Green;
        private static readonly Color DirectionMarkColor = Color.Lime;
        private static readonly Color SwordColor = Color.Aqua;

        private const char SymbolArrowUp = '\u25B2';
        private const char SymbolArrowDown = '\u25BC';
        private const char SymbolArrowLeft = '\u25C4';
        private const char SymbolArrowRight = '\u25BA';

        public SymbolsImage GetImage(object @object)
        {
            return GetImage((GoblinCreatureObject) @object);
        }

        private SymbolsImage GetImage(GoblinCreatureObject goblin)
        {
            var image = new SymbolsImage();


            image.SetPixel(1, 1, '\u263A', FaceColor, BodyColor);

            switch (goblin.Direction)
            {
                case Direction.Up:
                    image.SetPixel(1, 0, SymbolArrowUp, DirectionMarkColor);
                    image.SetPixel(0, 1, SymbolArrowLeft, BodyColor);
                    image.SetPixel(2, 1, SymbolArrowRight, BodyColor);
                    image.SetPixel(2, 0, '│', SwordColor);
                    break;
                case Direction.Down:
                    image.SetPixel(1, 2, SymbolArrowDown, DirectionMarkColor);
                    image.SetPixel(0, 1, SymbolArrowLeft, BodyColor);
                    image.SetPixel(2, 1, SymbolArrowRight, BodyColor);
                    image.SetPixel(0, 2, '│', SwordColor);
                    break;
                case Direction.Left:
                    image.SetPixel(0, 1, SymbolArrowLeft, DirectionMarkColor);
                    image.SetPixel(1, 0, SymbolArrowUp, BodyColor);
                    image.SetPixel(1, 2, SymbolArrowDown, BodyColor);
                    image.SetPixel(0, 0, '─', SwordColor);
                    break;
                case Direction.Right:
                    image.SetPixel(2, 1, SymbolArrowRight, DirectionMarkColor);
                    image.SetPixel(1, 0, SymbolArrowUp, BodyColor);
                    image.SetPixel(1, 2, SymbolArrowDown, BodyColor);
                    image.SetPixel(2, 2, '─', SwordColor);
                    break;
            }


            return image;
        }
    }
}