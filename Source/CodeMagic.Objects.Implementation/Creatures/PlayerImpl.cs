using System;
using CodeMagic.Core.Common;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation.Creatures
{
    public class PlayerImpl : Player, IImageProvider
    {
        private const string ImageUp = "Player_Up";
        private const string ImageDown = "Player_Down";
        private const string ImageLeft = "Player_Left";
        private const string ImageRight = "Player_Right";

        public PlayerImpl(PlayerConfiguration configuration) 
            : base(configuration)
        {
        }

        public SymbolsImage GetImage(IImagesStorage storage)
        {
            switch (Direction)
            {
                case Direction.Up:
                    return storage.GetImage(ImageUp);
                case Direction.Down:
                    return storage.GetImage(ImageDown);
                case Direction.Left:
                    return storage.GetImage(ImageLeft);
                case Direction.Right:
                    return storage.GetImage(ImageRight);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}