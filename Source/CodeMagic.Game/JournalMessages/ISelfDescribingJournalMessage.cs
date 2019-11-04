using System;
using System.Drawing;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Items;
using CodeMagic.Game.Statuses;

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

        protected static StyledString GetDamageText(int damage, Element element)
        {
            var color = ItemTextHelper.GetElementColor(element);
            var elementText = ItemTextHelper.GetElementName(element);
            return new StyledString($"{damage} {elementText} damage", color);
        }

        protected static string GetStatusName(string statusType)
        {
            switch (statusType)
            {
                case OverweightObjectStatus.StatusType:
                    return "Overweight";
                case OnFireObjectStatus.StatusType:
                    return "On Fire";
                case BlindObjectStatus.StatusType:
                    return "Blind";
                case ParalyzedObjectStatus.StatusType:
                    return "Paralyzed";
                case FrozenObjectStatus.StatusType:
                    return "Frozen";
                case WetObjectStatus.StatusType:
                    return "Wet";
                case OilyObjectStatus.StatusType:
                    return "Oily";
                case HungryObjectStatus.StatusType:
                    return "Hungry";
                default:
                    throw new ApplicationException($"Unknown object status type: {statusType}");
            }
        }
    }
}