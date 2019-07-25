using CodeMagic.Objects.Implementation;
using CodeMagic.UI.Images;

namespace CodeMagic.UI.Sad.Drawing.ImageProviding
{
    public class ImagesFactory
    {
        private readonly IImagesStorage imagesStorage;

        public ImagesFactory(IImagesStorage imagesStorage)
        {
            this.imagesStorage = imagesStorage;
        }

        public SymbolsImage GetImage(object objectToDraw)
        {
            if (objectToDraw is IImageProvider selfProvider)
            {
                return selfProvider.GetImage(imagesStorage);
            }

            return null;
        }
    }
}