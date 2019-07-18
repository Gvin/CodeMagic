using System.Drawing;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation
{
    public class PlayerDrawingProcessor : IDrawingProcessor
    {
        private const char SymbolArrowUp = '\u25B2';
        private const char SymbolArrowDown = '\u25BC';
        private const char SymbolArrowLeft = '\u25C4';
        private const char SymbolArrowRight = '\u25BA';

        private static readonly Color PlayerFrameColor = Color.Green;
        private static readonly Color PlayerFaceColor = Color.Yellow;
        private static readonly Color PlayerBodyColor = Color.BlueViolet;
        private static readonly Color DirectionMarkColor = Color.Lime;

        public SymbolsImage GetImage(object @object)
        {
            return Draw((IPlayer) @object);
        }

        private SymbolsImage Draw(IPlayer player)
        {
            var image = new SymbolsImage();
            image.SetDefaultColor(PlayerFrameColor);

            image.SetSymbolMap(new []
            {
                new char?[]{LineTypes.SingleDownRight, null, LineTypes.SingleDownLeft},
                new char?[]{null, null, null},
                new char?[]{LineTypes.SingleUpRight, null, LineTypes.SingleUpLeft }
            });

            image.SetPixel(1, 1, '\u263A', PlayerFaceColor, PlayerBodyColor);

            switch (player.Direction)
            {
                case Direction.Up:
                    image.SetPixel(1, 0, SymbolArrowUp, DirectionMarkColor);
                    image.SetPixel(0, 1, SymbolArrowLeft, PlayerBodyColor);
                    image.SetPixel(2, 1, SymbolArrowRight, PlayerBodyColor);
                    break;
                case Direction.Down:
                    image.SetPixel(1, 2, SymbolArrowDown, DirectionMarkColor);
                    image.SetPixel(0, 1, SymbolArrowLeft, PlayerBodyColor);
                    image.SetPixel(2, 1, SymbolArrowRight, PlayerBodyColor);
                    break;
                case Direction.Left:
                    image.SetPixel(0, 1, SymbolArrowLeft, DirectionMarkColor);
                    image.SetPixel(1, 0, SymbolArrowUp, PlayerBodyColor);
                    image.SetPixel(1, 2, SymbolArrowDown, PlayerBodyColor);
                    break;
                case Direction.Right:
                    image.SetPixel(2, 1, SymbolArrowRight, DirectionMarkColor);
                    image.SetPixel(1, 0, SymbolArrowUp, PlayerBodyColor);
                    image.SetPixel(1, 2, SymbolArrowDown, PlayerBodyColor);
                    break;
            }

            return image;
        }
    }
}