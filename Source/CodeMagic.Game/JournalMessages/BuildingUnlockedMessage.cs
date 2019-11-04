using CodeMagic.Game.Configuration.Buildings;
using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public class BuildingUnlockedMessage : SelfDescribingJournalMessage
    {
        private readonly IBuildingConfiguration building;

        public BuildingUnlockedMessage(IBuildingConfiguration building)
        {
            this.building = building;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                new StyledString("New building unlocked: ["),
                new StyledString(building.Name, ItemDrawingHelper.GetRarenessColor(building.Rareness)),
                new StyledString("]"),
            };
        }
    }
}