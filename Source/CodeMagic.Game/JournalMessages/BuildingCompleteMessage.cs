using CodeMagic.Game.Configuration.Buildings;
using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class BuildingCompleteMessage : SelfDescribingJournalMessage
    {
        private readonly IBuildingConfiguration building;

        public BuildingCompleteMessage(IBuildingConfiguration building)
        {
            this.building = building;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                new StyledString("Building ["),
                new StyledString(building.Name, ItemDrawingHelper.GetRarenessColor(building.Rareness)),
                new StyledString("] is complete."),
            };
        }
    }
}