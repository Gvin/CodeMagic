using CodeMagic.Core.Configuration.Buildings;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class BuildingSitePlacesMessage : IJournalMessage
    {
        public BuildingSitePlacesMessage(IBuildingConfiguration building)
        {
            Building = building;
        }

        public IBuildingConfiguration Building { get; }
    }
}