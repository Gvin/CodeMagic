using System.Collections.Generic;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class ResourcesRequiredToBuildMessage : IJournalMessage
    {
        public ResourcesRequiredToBuildMessage(Dictionary<string, int> remainingResources, int remainingTime)
        {
            RemainingResources = remainingResources;
            RemainingTime = remainingTime;
        }

        public Dictionary<string, int> RemainingResources { get; }

        public int RemainingTime { get; }
    }
}