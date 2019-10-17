using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Game.JournalMessages
{
    public class ResourcesRequiredToBuildMessage : SelfDescribingJournalMessage
    {
        private readonly int remainingTime;
        private readonly Dictionary<string, int> remainingResources;

        public ResourcesRequiredToBuildMessage(Dictionary<string, int> remainingResources, int remainingTime)
        {
            this.remainingResources = remainingResources;
            this.remainingTime = remainingTime;
        }

        public override StyledLine GetDescription()
        {
            if (remainingResources == null || remainingResources.Count == 0)
            {
                return new StyledLine
                {
                    new StyledString($"{remainingTime} turns remaining to finish building."),
                };
            }

            var resourcesString = string.Join(", ",
                remainingResources.Select(resource => $"{resource.Value} {resource.Key}"));
            return new StyledLine
            {
                new StyledString($"{remainingTime} turns and the following resources are required to finish this building: {resourcesString}")
            };
        }
    }
}