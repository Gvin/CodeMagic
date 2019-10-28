using System.Drawing;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;

namespace CodeMagic.Game.JournalMessages
{
    public interface ISelfDescribingJournalMessage : IJournalMessage
    {
        StyledLine GetDescription();
    }

    public abstract class SelfDescribingJournalMessage : ISelfDescribingJournalMessage
    {
        protected const string PlayerName = "You";

        protected static readonly Color TextColor = Color.White;
        protected static readonly Color SpellNameColor = Color.BlueViolet;
        protected static readonly Color ManaColor = Color.Blue;
        protected static readonly Color ErrorColor = Color.Red;
        protected static readonly Color SpellLogMessageColor = Color.Gray;
        protected static readonly Color HealValueColor = Color.Green;

        protected static readonly StyledString ManaString = new StyledString("Mana", ManaColor);

        public abstract StyledLine GetDescription();

        protected static string GetMapObjectName(IMapObject mapObject)
        {
            if (mapObject is IPlayer)
                return PlayerName;

            return mapObject.Name;
        }

        protected static StyledString GetItemNameText(IItem item)
        {
            return new StyledString(item.Name, ItemDrawingHelper.GetItemColor(item));
        }
    }
}