using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Statuses;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;

namespace CodeMagic.UI.Sad.Drawing
{
    public class JournalMessageFormatter
    {
        private const string PlayerName = "You";

        private static readonly Color TurnsCountColor = Color.Gray;
        private static readonly Color TextColor = Color.White;
        private static readonly Color BackgroundColor = Color.Black;
        private static readonly Color SpellNameColor = Color.BlueViolet;
        private static readonly Color ManaColor = Color.Blue;
        private static readonly Color ErrorColor = Color.Red;
        private static readonly Color SpellLogMessageColor = Color.Gray;

        private static readonly ColoredString ManaString = new ColoredString("Mana", ManaColor, BackgroundColor);

        public ColoredString[] FormatMessage(JournalMessageData messageData)
        {
            var formattedMessage = new List<ColoredString>
            {
                new ColoredString($"[{messageData.Turn}] ", TurnsCountColor, BackgroundColor)
            };
            formattedMessage.AddRange(GetMessageBody(messageData.Message));
            return formattedMessage.ToArray();
        }

        private ColoredString[] GetMessageBody(IJournalMessage message)
        {
            if (message is ISelfDescribingJournalMessage selfDescribingMessage)
            {
                return selfDescribingMessage.GetDescription().Parts.Select(styledString =>
                        new ColoredString(styledString.String.ConvertGlyphs(),
                            new Cell(styledString.TextColor.ToXna(), BackgroundColor)))
                    .ToArray();
            }

            switch (message)
            {
                case EnvironmentDamageMessage environmentDamageMessage:
                    return GetEnvironmentDamageMessage(environmentDamageMessage);
                case DeathMessage deathMessage:
                    return GetDeathMessage(deathMessage);
                case BurningDamageMessage burningDamageMessage:
                    return GetBurningDamageMessage(burningDamageMessage);
                case DealDamageMessage dealDamageMessage:
                    return GetDealDamageMessage(dealDamageMessage);
                case SpellOutOfManaMessage spellOutOfManaMessage:
                    return GetSpellOutOfManaMessage(spellOutOfManaMessage);
                case SpellErrorMessage spellErrorMessage:
                    return GetSpellErrorMessage(spellErrorMessage);
                case SpellLogMessage spellLogMessage:
                    return GetSpellLogMessage(spellLogMessage);
                case AttackMissMessage attackMissMessage:
                    return GetAttackMissMessage(attackMissMessage);
                case StatusAddedMessage statusAddedMessage:
                    return GetStatusAddedMessage(statusAddedMessage);
                case DamageBlockedMessage damageBlockedMessage:
                    return GetDamageBlockedMessage(damageBlockedMessage);
                case DungeonLevelMessage dungeonLevelMessage:
                    return GetDungeonLevelMessage(dungeonLevelMessage);
                default:
                    throw new ApplicationException($"Unknown journal message type: {message.GetType().FullName}");
            }
        }

        private ColoredString[] GetDungeonLevelMessage(DungeonLevelMessage message)
        {
            return new[]
            {
                new ColoredString($"{PlayerName} reached {message.Level} level of the dungeon"), 
            };
        }

        private ColoredString[] GetDamageBlockedMessage(DamageBlockedMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Target)} blocked "),
                GetDamageText(message.BlockedValue, message.DamageElement)
            };
        }

        private ColoredString[] GetStatusAddedMessage(StatusAddedMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Target)} got [{GetStatusName(message.StatusType)}] status", TextColor, BackgroundColor) 
            };
        }

        private ColoredString[] GetAttackMissMessage(AttackMissMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Source)} missed {GetMapObjectName(message.Target)}", 
                    TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetSpellLogMessage(SpellLogMessage message)
        {
            return new[]
            {
                new ColoredString("Spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName.ConvertGlyphs(), SpellNameColor, BackgroundColor),
                new ColoredString(" sent a message: ", TextColor, BackgroundColor),
                new ColoredString(message.Message, SpellLogMessageColor, BackgroundColor)
            };
        }

        private ColoredString[] GetSpellErrorMessage(SpellErrorMessage message)
        {
            return new[]
            {
                new ColoredString("An error occured in spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName.ConvertGlyphs(), SpellNameColor, BackgroundColor),
                new ColoredString(": ", TextColor, BackgroundColor),
                new ColoredString(message.Message, ErrorColor, BackgroundColor)
            };
        }

        private ColoredString[] GetSpellOutOfManaMessage(SpellOutOfManaMessage message)
        {
            return new[]
            {
                new ColoredString("Spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName.ConvertGlyphs(), SpellNameColor, BackgroundColor),
                new ColoredString(" is out of ", TextColor, BackgroundColor),
                ManaString
            };
        }

        private ColoredString[] GetDealDamageMessage(DealDamageMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Source)} dealt ", TextColor, BackgroundColor),
                GetDamageText(message.Damage, message.Element),
                new ColoredString($" to {GetMapObjectName(message.Target)}", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetBurningDamageMessage(BurningDamageMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Object)} got ", TextColor, BackgroundColor),
                GetDamageText(message.Damage, Element.Fire),
                new ColoredString(" from burning", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetDeathMessage(DeathMessage message)
        {
            var deathMessage = message.Object is ICreatureObject ? "died" : "destroyed";

            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Object)} {deathMessage}", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetEnvironmentDamageMessage(EnvironmentDamageMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Object)} got ", TextColor, BackgroundColor),
                GetDamageText(message.Damage, message.Element),
                new ColoredString(" from environment", TextColor, BackgroundColor)
            };
        }

        private string GetMapObjectName(IMapObject mapObject)
        {
            if (mapObject is IPlayer)
                return PlayerName;

            return mapObject.Name;
        }

        private ColoredString GetDamageText(int damage, Element element)
        {
            var color = ItemTextHelper.GetElementColor(element);
            var elementText = ItemTextHelper.GetElementName(element);
            return new ColoredString($"{damage} {elementText} damage", color.ToXna(), BackgroundColor);
        }

        private string GetStatusName(string statusType)
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