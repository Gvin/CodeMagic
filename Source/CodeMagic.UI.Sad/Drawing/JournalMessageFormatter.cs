using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;
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

        private static readonly Color PhysicalDamageColor = Color.LightGray;
        private static readonly Color FireDamageColor = Color.Orange;
        private static readonly Color FrostDamageColor = Color.LightBlue;
        private static readonly Color AcidDamageColor = Color.Lime;

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
                case NotEnoughManaMessage notEnoughManaMessage:
                    return GetNotEnoughManaMessage(notEnoughManaMessage);
                case SpellCastMessage spellCastMessage:
                    return GetSpellCastMessage(spellCastMessage);
                case SpellErrorMessage spellErrorMessage:
                    return GetSpellErrorMessage(spellErrorMessage);
                case SpellLogMessage spellLogMessage:
                    return GetSpellLogMessage(spellLogMessage);
                default:
                    throw new ApplicationException($"Unknown journal message type: {message.GetType().FullName}");
            }
        }

        private ColoredString[] GetSpellLogMessage(SpellLogMessage message)
        {
            return new[]
            {
                new ColoredString("Spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName, SpellNameColor, BackgroundColor),
                new ColoredString(" sent a message: ", TextColor, BackgroundColor),
                new ColoredString(message.Message, SpellLogMessageColor, BackgroundColor)
            };
        }

        private ColoredString[] GetSpellErrorMessage(SpellErrorMessage message)
        {
            return new[]
            {
                new ColoredString("An error occured in spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName, SpellNameColor, BackgroundColor),
                new ColoredString(": ", TextColor, BackgroundColor),
                new ColoredString(message.Message, ErrorColor, BackgroundColor)
            };
        }

        private ColoredString[] GetSpellCastMessage(SpellCastMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Caster)} casted spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName, SpellNameColor, BackgroundColor)
            };
        }

        private ColoredString[] GetNotEnoughManaMessage(NotEnoughManaMessage message)
        {
            return new[]
            {
                new ColoredString("You have not enough ", TextColor, BackgroundColor),
                ManaString,
                new ColoredString(" to cast this spell", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetSpellOutOfManaMessage(SpellOutOfManaMessage message)
        {
            return new[]
            {
                new ColoredString("Spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName, SpellNameColor, BackgroundColor),
                new ColoredString(" is out of ", TextColor, BackgroundColor),
                ManaString
            };
        }

        private ColoredString[] GetDealDamageMessage(DealDamageMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Source)} dealt ", TextColor, BackgroundColor),
                GetDamageText(message.Damage, null),
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
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Object)} is dead", TextColor, BackgroundColor)
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

        private ColoredString GetDamageText(int damage, Element? element)
        {
            var color = GetDamageElementColor(element);
            var elementText = GetDamageElementText(element);
            return new ColoredString($"{damage} {elementText} damage", color, BackgroundColor);
        }

        private Color GetDamageElementColor(Element? element)
        {
            if (!element.HasValue)
                return PhysicalDamageColor;

            switch (element.Value)
            {
                case Element.Fire:
                    return FireDamageColor;
                case Element.Frost:
                    return FrostDamageColor;
                case Element.Acid:
                    return AcidDamageColor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), $"Unknown damage element: {element}");
            }
        }

        private string GetDamageElementText(Element? element)
        {
            if (!element.HasValue)
                return "Physical";

            switch (element.Value)
            {
                case Element.Fire:
                    return "Fire";
                case Element.Frost:
                    return "Frost";
                case Element.Acid:
                    return "Acid";
                default:
                    throw new ArgumentOutOfRangeException(nameof(element), $"Unknown damage element: {element}");
            }
        }
    }
}