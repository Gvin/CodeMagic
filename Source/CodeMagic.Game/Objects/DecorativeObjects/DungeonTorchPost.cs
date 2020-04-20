using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.DecorativeObjects
{
    public class DungeonTorchPost : MapObjectBase, ILightObject, IWorldImageProvider
    {
        private const string AnimationName = "Decoratives_TorchPost";

        private readonly AnimationsBatchManager animationsManager;

        public DungeonTorchPost(SaveData data) 
            : base(data)
        {
            animationsManager = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
        }

        public DungeonTorchPost() 
            : base("Torch Post")
        {
            animationsManager = new AnimationsBatchManager(TimeSpan.FromMilliseconds(500), AnimationFrameStrategy.Random);
        }

        public override ZIndex ZIndex => ZIndex.BigDecoration;

        public override ObjectSize Size => ObjectSize.Huge;

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