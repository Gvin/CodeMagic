using System;
using CodeMagic.Core.Game;
using CodeMagic.UI.Images;

namespace CodeMagic.Objects.Implementation
{
    public class AnimationManager
    {
        private readonly TimeSpan frameLifeTime;
        private int currentFrameIndex;
        private readonly SymbolsImage[] frames;
        private DateTime lastUpdated;

        public AnimationManager(SymbolsImage[] frames, TimeSpan frameLifeTime, bool randomStartFrame = false)
        {
            this.frames = frames;
            this.frameLifeTime = frameLifeTime;

            if (randomStartFrame)
            {
                currentFrameIndex = RandomHelper.GetRandomValue(0, frames.Length - 1);
            }
            else
            {
                currentFrameIndex = 0;
            }

            lastUpdated = DateTime.Now;
        }

        public SymbolsImage GetCurrentFrame()
        {
            lock (frames)
            {
                var updateInterval = DateTime.Now - lastUpdated;
                var diffFrames = (int)Math.Truncate(updateInterval.TotalMilliseconds / frameLifeTime.TotalMilliseconds);
                if (diffFrames > 0)
                {
                    for (var index = 0; index < diffFrames; index++)
                    {
                        IncrementFrameIndex();
                    }
                    lastUpdated = DateTime.Now;
                }

                return frames[currentFrameIndex];
            }
        }

        private void IncrementFrameIndex()
        {
            currentFrameIndex++;
            if (currentFrameIndex >= frames.Length)
            {
                currentFrameIndex = 0;
            }
        }
    }
}