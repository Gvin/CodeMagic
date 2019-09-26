using CodeMagic.Core.Configuration.Buildings;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class BuildingCompleteMessage : IJournalMessage
    {
        public BuildingCompleteMessage(IBuildingConfiguration building)
        {
            Building = building;
        }

        public IBuildingConfiguration Building { get; }
    }
}