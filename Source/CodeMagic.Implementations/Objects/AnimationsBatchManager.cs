using System;
using System.Collections.Generic;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects
{
    public class AnimationsBatchManager
    {
        private readonly Dictionary<string, AnimationManager> managers;

        private readonly TimeSpan changeInterval;
        private readonly AnimationFrameStrategy frameStrategy;

        public AnimationsBatchManager(
            TimeSpan changeInterval,
            AnimationFrameStrategy frameStrategy = AnimationFrameStrategy.OneByOneStartFromZero)
        {
            this.changeInterval = changeInterval;
            this.frameStrategy = frameStrategy;
            managers = new Dictionary<string, AnimationManager>();
        }

        public SymbolsImage GetImage(IImagesStorage storage, string animationName)
        {
            if (!managers.ContainsKey(animationName))
            {
                managers.Add(animationName,
                    new AnimationManager(storage.GetAnimation(animationName), changeInterval, frameStrategy));
            }

            return managers[animationName].GetCurrentFrame();
        }
    }
}