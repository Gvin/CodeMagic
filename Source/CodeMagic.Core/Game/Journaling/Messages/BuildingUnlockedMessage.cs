using CodeMagic.Core.Configuration.Buildings;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class BuildingUnlockedMessage : IJournalMessage
    {
        public BuildingUnlockedMessage(IBuildingConfiguration building)
        {
            Building = building;
        }

        public IBuildingConfiguration Building { get; }
    }
}