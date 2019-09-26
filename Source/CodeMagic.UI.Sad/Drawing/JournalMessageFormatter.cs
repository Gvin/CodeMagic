using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Statuses;
using CodeMagic.Implementations.Items;
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
        private static readonly Color HealValueColor = Color.Green;

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
                case NotEnoughManaToCastMessage notEnoughManaMessage:
                    return GetNotEnoughManaToCastMessage(notEnoughManaMessage);
                case SpellCastMessage spellCastMessage:
                    return GetSpellCastMessage(spellCastMessage);
                case SpellErrorMessage spellErrorMessage:
                    return GetSpellErrorMessage(spellErrorMessage);
                case SpellLogMessage spellLogMessage:
                    return GetSpellLogMessage(spellLogMessage);
                case AttackMissMessage attackMissMessage:
                    return GetAttackMissMessage(attackMissMessage);
                case ParalyzedMessage paralyzedMessage:
                    return GetParalyzedMessage(paralyzedMessage);
                case StatusAddedMessage statusAddedMessage:
                    return GetStatusAddedMessage(statusAddedMessage);
                case UsedItemMessage usedItemMessage:
                    return GetUsedItemMessage(usedItemMessage);
                case HealedMessage healedMessage:
                    return GetHealedMessage(healedMessage);
                case ManaRestoredMessage manaRestoredMessage:
                    return GetManaRestoredMessage(manaRestoredMessage);
                case DamageBlockedMessage damageBlockedMessage:
                    return GetDamageBlockedMessage(damageBlockedMessage);
                case OverweightBlocksMovementMessage overweightBlocksMovementMessage:
                    return GetOverweightBlocksMovementMessage(overweightBlocksMovementMessage);
                case DungeonLevelMessage dungeonLevelMessage:
                    return GetDungeonLevelMessage(dungeonLevelMessage);
                case NotEnoughManaToScrollMessage notEnoughManaToScrollMessage:
                    return GetNotEnoughManaToScrollMessage(notEnoughManaToScrollMessage);
                case FailedToUseScrollMessage failedToUseScrollMessage:
                    return GetFailedToUseScrollMessage(failedToUseScrollMessage);
                case CastNotAllowedMessage castNotAllowedMessage:
                    return GetCastNotAllowedMessage(castNotAllowedMessage);
                case FightNotAllowedMessage fightNotAllowedMessage:
                    return GetFightNotAllowedMessage(fightNotAllowedMessage);
                case StoringLocationNotAllowedMessage storingLocationNotAllowedMessage:
                    return GetStoringLocationNotAllowedMessage(storingLocationNotAllowedMessage);
                case BuildingCompleteMessage buildingCompleteMessage:
                    return GetBuildingCompleteMessage(buildingCompleteMessage);
                case ResourcesRequiredToBuildMessage resourcesRequiredToBuildMessage:
                    return GetResourcesRequiredToBuildMessage(resourcesRequiredToBuildMessage);
                case BuildingSiteRemovedMessage buildingSiteRemovedMessage:
                    return GetBuildingSiteRemovedMessage(buildingSiteRemovedMessage);
                case CellBlockedForBuildingMessage cellBlockedForBuildingMessage:
                    return GetCellBlockedForBuildingMessage(cellBlockedForBuildingMessage);
                case BuildingSitePlacesMessage buildingSitePlacesMessage:
                    return GetBuildingSitePlacesMessage(buildingSitePlacesMessage);
                case BuildingUnlockedMessage buildingUnlockedMessage:
                    return GetBuildingUnlockedMessage(buildingUnlockedMessage);
                default:
                    throw new ApplicationException($"Unknown journal message type: {message.GetType().FullName}");
            }
        }

        private ColoredString[] GetBuildingUnlockedMessage(BuildingUnlockedMessage message)
        {
            return new[]
            {
                new ColoredString("New building unlocked: ["),
                new ColoredString(message.Building.Name, new Cell(ItemDrawingHelper.GetRarenessColor(message.Building.Rareness))), 
                new ColoredString("]"), 
            };
        }

        private ColoredString[] GetBuildingSitePlacesMessage(BuildingSitePlacesMessage message)
        {
            return new[]
            {
                new ColoredString("["),
                new ColoredString(message.Building.Name, new Cell(ItemDrawingHelper.GetRarenessColor(message.Building.Rareness))),
                new ColoredString("] building site is placed"),
            };
        }

        private ColoredString[] GetCellBlockedForBuildingMessage(CellBlockedForBuildingMessage message)
        {
            return new[]
            {
                new ColoredString("Target area cannot be used for building"),
            };
        }

        private ColoredString[] GetBuildingSiteRemovedMessage(BuildingSiteRemovedMessage message)
        {
            return new[]
            {
                new ColoredString("Building site removed"),
            };
        }

        private ColoredString[] GetResourcesRequiredToBuildMessage(ResourcesRequiredToBuildMessage message)
        {
            if (message.RemainingResources == null || message.RemainingResources.Count == 0)
            {
                return new[]
                {
                    new ColoredString($"{message.RemainingTime} turns remaining to finish building."),
                };
            }

            var resourcesString = string.Join(", ",
                message.RemainingResources.Select(resource => $"{resource.Value} {resource.Key}"));
            return new []
            {
                new ColoredString($"{message.RemainingTime} turns and the following resources are required to finish this building: {resourcesString}")
            };
        }

        private ColoredString[] GetBuildingCompleteMessage(BuildingCompleteMessage message)
        {
            return new[]
            {
                new ColoredString($"Building ["),
                new ColoredString(message.Building.Name, new Cell(ItemDrawingHelper.GetRarenessColor(message.Building.Rareness))),
                new ColoredString("] is complete."), 
            };
        }

        private ColoredString[] GetStoringLocationNotAllowedMessage(StoringLocationNotAllowedMessage message)
        {
            return new[]
            {
                new ColoredString("This area is too unstable to store its location."),
            };
        }

        private ColoredString[] GetFightNotAllowedMessage(FightNotAllowedMessage message)
        {
            return new[]
            {
                new ColoredString("Fighting is not allowed in this area"),
            };
        }

        private ColoredString[] GetCastNotAllowedMessage(CastNotAllowedMessage message)
        {
            return new[]
            {
                new ColoredString("Casting is not allowed in this area"),
            };
        }

        private ColoredString[] GetFailedToUseScrollMessage(FailedToUseScrollMessage message)
        {
            return new[]
            {
                new ColoredString($"{PlayerName} failed to use scroll. It's destroyed")
            };
        }

        private ColoredString[] GetNotEnoughManaToScrollMessage(NotEnoughManaToScrollMessage message)
        {
            return new[]
            {
                new ColoredString($"{PlayerName} have not enough ", TextColor, BackgroundColor),
                ManaString,
                new ColoredString(" to create a scroll with this spell", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetDungeonLevelMessage(DungeonLevelMessage message)
        {
            return new[]
            {
                new ColoredString($"{PlayerName} reached {message.Level} level of the dungeon"), 
            };
        }

        private ColoredString[] GetOverweightBlocksMovementMessage(OverweightBlocksMovementMessage message)
        {
            return new[]
            {
                new ColoredString($"Inventory is too heavy and {PlayerName} cannot move"),
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

        private ColoredString[] GetHealedMessage(HealedMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Target)} restored ", TextColor, BackgroundColor),
                new ColoredString(message.HealValue.ToString(), HealValueColor, BackgroundColor),
                new ColoredString(" health", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetManaRestoredMessage(ManaRestoredMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Target)} restored ", TextColor, BackgroundColor),
                new ColoredString(message.ManaRestoreValue.ToString(), ManaColor, BackgroundColor),
                new ColoredString(" mana", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetUsedItemMessage(UsedItemMessage message)
        {
            return new[]
            {
                new ColoredString($"{PlayerName} used [", TextColor, BackgroundColor),
                new ColoredString(message.Item.Name.ConvertGlyphs(), ItemDrawingHelper.GetItemColor(message.Item), BackgroundColor),
                new ColoredString("]", TextColor, BackgroundColor)
            };
        }

        private ColoredString[] GetStatusAddedMessage(StatusAddedMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Target)} got [{GetStatusName(message.StatusType)}] status", TextColor, BackgroundColor) 
            };
        }

        private ColoredString[] GetParalyzedMessage(ParalyzedMessage message)
        {
            return new[]
            {
                new ColoredString($"{PlayerName} paralyzed and cannot move or attack")
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

        private ColoredString[] GetSpellCastMessage(SpellCastMessage message)
        {
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Caster)} casted spell ", TextColor, BackgroundColor),
                new ColoredString(message.SpellName.ConvertGlyphs(), SpellNameColor, BackgroundColor)
            };
        }

        private ColoredString[] GetNotEnoughManaToCastMessage(NotEnoughManaToCastMessage message)
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
            return new[]
            {
                new ColoredString($"{GetMapObjectName(message.Object)} died", TextColor, BackgroundColor)
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
                default:
                    throw new ApplicationException($"Unknown object status type: {statusType}");
            }
        }
    }
}