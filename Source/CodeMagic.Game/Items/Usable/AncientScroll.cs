using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public class AncientScroll : ScrollBase
    {
        private const string SaveKeyDamagePercent = "DamagePercent";
        private const string SaveKeyDamagedCode = "DamagedCode";
        private const string SaveKeyInventoryImageName = "InventoryImageName";

        private const int MagicDamageOnFailedScroll = 1;
        private const int DisturbanceIncrementOnFailedScroll = 5;

        private const char DamagedSymbol = '▒';
        private const string NewLineSign = "\r\n";
        private const int MinDamageZoneLength = 3;
        private const int MaxDamageZoneLength = 10;

        private const string ImageWorld = "ItemsOnGround_Scroll";

        private const string ImageInventory1 = "Item_Scroll_Old_V1";
        private const string ImageInventory2 = "Item_Scroll_Old_V2";
        private const string ImageInventory3 = "Item_Scroll_Old_V3";

        private readonly string inventoryImageName;

        private readonly int damagePercent;
        private readonly string damagedCode;

        public AncientScroll(SaveData data) : base(data)
        {
            damagePercent = data.GetIntValue(SaveKeyDamagePercent);
            damagedCode = data.GetStringValue(SaveKeyDamagedCode);
            inventoryImageName = data.GetStringValue(SaveKeyInventoryImageName);
        }

        public AncientScroll(AncientScrollItemConfiguration configuration) : base(configuration)
        {
            damagePercent = configuration.DamagePercent;
            damagedCode = GenerateDamagedCode(configuration.Code, configuration.DamagePercent);
            inventoryImageName = GetInventoryImageName(configuration.Code);
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyInventoryImageName, inventoryImageName);
            data.Add(SaveKeyDamagePercent, damagePercent);
            data.Add(SaveKeyDamagedCode, damagedCode);
            return data;
        }

        private static string GetInventoryImageName(string code)
        {
            var letterA = code.Count(c => char.ToLower(c) == 'a');
            var letterB = code.Count(c => char.ToLower(c) == 'b');
            var letterC = code.Count(c => char.ToLower(c) == 'c');

            if (letterA > letterB && letterA > letterC)
                return ImageInventory1;
            if (letterB > letterA && letterB > letterC)
                return ImageInventory2;
            return ImageInventory3;
        }

        public override bool Use(GameCore<Player> game)
        {
            var chanceToRead = GetChanceToRead(game.Player);
            if (RandomHelper.CheckChance(chanceToRead))
                return base.Use(game);

            game.Journal.Write(new FailedToUseScrollMessage());
            game.Player.Damage(CurrentGame.PlayerPosition, MagicDamageOnFailedScroll, Element.Magic);
            game.Journal.Write(new EnvironmentDamageMessage(game.Player, MagicDamageOnFailedScroll, Element.Magic));
            var environment = (GameEnvironment) game.Map.GetCell(game.PlayerPosition).Environment;
            environment.MagicDisturbanceLevel += DisturbanceIncrementOnFailedScroll;
            return false;
        }

        private int GetChanceToRead(Player player)
        {
            return 100 - damagePercent + player.ScrollReadingBonus;
        }

        private static string GenerateDamagedCode(string code, int damagePercent)
        {
            var lines = code.Split(new[] {NewLineSign}, StringSplitOptions.None);
            var totalSymbolsCount = lines.Sum(line => line.Length);
            var remainingDamageSymbols = (int) Math.Round(totalSymbolsCount * damagePercent / 100d);

            const int maxIterations = 1000;
            var iteration = 0;

            while (remainingDamageSymbols > 0 && iteration < maxIterations)
            {
                iteration++;

                var damageZoneLength = RandomHelper.GetRandomValue(MinDamageZoneLength, MaxDamageZoneLength);
                var lineIndex = RandomHelper.GetRandomValue(0, lines.Length - 1);
                var line = lines[lineIndex];
                if (line.Length < damageZoneLength)
                    continue;

                var zoneStartPosition = RandomHelper.GetRandomValue(0, line.Length - 1 - damageZoneLength);
                if (line[zoneStartPosition] == DamagedSymbol)
                    continue;

                for (var index = 0; index < damageZoneLength; index++)
                {
                    var onLineIndex = index + zoneStartPosition;
                    line = line.Remove(onLineIndex, 1);
                    line = line.Insert(onLineIndex, DamagedSymbol.ToString());
                }

                lines[lineIndex] = line;
                remainingDamageSymbols -= damageZoneLength;
            }

            return string.Join(NewLineSign, lines);
        }

        public override string GetSpellCode()
        {
            return damagedCode;
        }

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageWorld);
        }

        public override SymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage(inventoryImageName);
        }

        public override StyledLine[] GetDescription(Player player)
        {
            return new[]
            {
                TextHelper.GetWeightLine(Weight),
                StyledLine.Empty,
                new StyledLine {$"Spell Name: {SpellName}"},
                new StyledLine {"Damaged: ", new StyledString($"{damagePercent}%", TextHelper.NegativeValueColor)},
                StyledLine.Empty,
                new StyledLine {"This scroll looks old and damaged."},
                new StyledLine {"It's title is written with some unknown language."},
                new StyledLine {"Some runes can barely be red."}
            };
        }
    }

    public class AncientScrollItemConfiguration : ScrollItemConfiguration
    {
        public int DamagePercent { get; set; }
    }
}