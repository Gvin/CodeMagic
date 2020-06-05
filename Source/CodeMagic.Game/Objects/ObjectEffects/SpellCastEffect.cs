using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.ObjectEffects
{
    public class SpellCastEffect : ObjectEffect
    {
        private const string ImageName = "Effect_SpellCast";

        public override SymbolsImage GetEffectImage(int width, int height, IImagesStorage imagesStorage)
        {
            return imagesStorage.GetImage(ImageName);
        }
    }
}