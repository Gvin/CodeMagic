using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.DecorativeObjects
{
    public class DungeonTorchPost : IMapObject, ILightObject, IWorldImageProvider
    {
        private const string AnimationName = "Decoratives_TorchPost";

        private readonly AnimationsBatchManager animationsManager;

        public DungeonTorchPost()
        {
            animationsManager = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
        }

        public string Name => "Torch Post";
        public bool BlocksMovement => false;
        public bool BlocksProjectiles => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksAttack => false;
        public bool BlocksEnvironment => false;
        public ZIndex ZIndex => ZIndex.BigDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightLevel.Medium),
        };

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return animationsManager.GetImage(storage, AnimationName);
        }
    }
}