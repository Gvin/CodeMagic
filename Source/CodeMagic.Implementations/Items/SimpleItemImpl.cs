using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Items
{
    public abstract class SimpleItemImpl : Item, IImageProvider
    {
        private readonly string imageName;

        protected SimpleItemImpl(SimpleItemConfiguration configuration) 
            : base(configuration)
        {
            imageName = configuration.ImageName;
        }
        public SymbolsImage GetImage(IImagesStorage storage)
        {
            return storage.GetImage(imageName);
        }
    }

    public class SimpleItemConfiguration : ItemConfiguration
    {
        public string ImageName { get; set; }
    }
}