using System;
using System.Collections.Generic;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation
{
    public class AnimationsBatchManager
    {
        private readonly Dictionary<string, AnimationManager> managers;

        private readonly TimeSpan changeInterval;
        private readonly bool randomStartFrame;

        public AnimationsBatchManager(TimeSpan changeInterval, bool randomStartFrame)
        {
            this.changeInterval = changeInterval;
            this.randomStartFrame = randomStartFrame;
            managers = new Dictionary<string, AnimationManager>();
        }

        public SymbolsImage GetImage(IImagesStorage storage, string animationName)
        {
            if (!managers.ContainsKey(animationName))
            {
                managers.Add(animationName,
                    new AnimationManager(storage.GetAnimation(animationName), changeInterval, randomStartFrame));
            }

            return managers[animationName].GetCurrentFrame();
        }
    }
}