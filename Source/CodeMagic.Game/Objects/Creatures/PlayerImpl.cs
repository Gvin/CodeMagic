using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Creatures
{
    public class PlayerImpl : Player, IWorldImageProvider
    {
        private const string ImageUp = "Player_Up";
        private const string ImageDown = "Player_Down";
        private const string ImageLeft = "Player_Left";
        private const string ImageRight = "Player_Right";

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            switch (Direction)
            {
                case Direction.North:
                    return storage.GetImage(ImageUp);
                case Direction.South:
                    return storage.GetImage(ImageDown);
                case Direction.West:
                    return storage.GetImage(ImageLeft);
                case Direction.East:
                    return storage.GetImage(ImageRight);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}