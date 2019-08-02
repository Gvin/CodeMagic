using System;
using CodeMagic.Core.Game;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects
{
    public class AnimationManager
    {
        private readonly TimeSpan frameLifeTime;
        private int currentFrameIndex;
        private readonly SymbolsImage[] frames;
        private DateTime lastUpdated;
        private readonly AnimationFrameStrategy frameStrategy;

        public AnimationManager(
            SymbolsImage[] frames,
            TimeSpan frameLifeTime,
            AnimationFrameStrategy frameStrategy = AnimationFrameStrategy.OneByOneStartFromZero)
        {
            this.frames = frames;
            this.frameLifeTime = frameLifeTime;
            this.frameStrategy = frameStrategy;

            currentFrameIndex = GetInitialFrameIndex();

            lastUpdated = DateTime.Now;
        }

        private int GetInitialFrameIndex()
        {
            switch (frameStrategy)
            {
                case AnimationFrameStrategy.OneByOneStartFromZero:
                    return 0;
                case AnimationFrameStrategy.Random:
                case AnimationFrameStrategy.OneByOneStartFromRandom:
                    return GetRandomFrameIndex();
                default:
                    throw new ArgumentOutOfRangeException($"Unknown frame strategy: {frameStrategy}");
            }
        }

        private int GetNextFrameIndex()
        {
            switch (frameStrategy)
            {
                case AnimationFrameStrategy.OneByOneStartFromRandom:
                case AnimationFrameStrategy.OneByOneStartFromZero:
                    if (currentFrameIndex >= frames.Length - 2)
                    {
                        return 0;
                    }
                    return currentFrameIndex + 1;
                case AnimationFrameStrategy.Random:
                    return GetRandomFrameIndex();
                default:
                    throw new ArgumentOutOfRangeException($"Unknown frame strategy: {frameStrategy}");
            }
        }

        private int GetRandomFrameIndex()
        {
            return RandomHelper.GetRandomValue(0, frames.Length - 1);
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
                        currentFrameIndex = GetNextFrameIndex();
                    }
                    lastUpdated = DateTime.Now;
                }

                return frames[currentFrameIndex];
            }
        }
    }

    public enum AnimationFrameStrategy
    {
        OneByOneStartFromZero,
        OneByOneStartFromRandom,
        Random
    }
}