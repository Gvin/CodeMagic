using CodeMagic.Game.Configuration.Buildings;
using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class BuildingSitePlacesMessage : SelfDescribingJournalMessage
    {
        private readonly IBuildingConfiguration building;

        public BuildingSitePlacesMessage(IBuildingConfiguration building)
        {
            this.building = building;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                new StyledString("["),
                new StyledString(building.Name, ItemDrawingHelper.GetRarenessColor(building.Rareness)),
                new StyledString("] building site is placed"),
            };
        }
    }
}