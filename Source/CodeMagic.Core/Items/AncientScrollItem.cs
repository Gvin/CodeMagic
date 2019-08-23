using System;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;

namespace CodeMagic.Core.Items
{
    public abstract class AncientScrollItem : ScrollItem
    {
        private const int MagicDamageOnFailedScroll = 1;
        private const int DisturbanceIncrementOnFailedScroll = 5;

        private const char DamagedSymbol = '▒';
        private const string NewLineSign = "\r\n";
        private const int MinDamageZoneLength = 3;
        private const int MaxDamageZoneLength = 10;

        protected readonly int DamagePercent;
        private readonly string damagedCode;

        protected AncientScrollItem(AncientScrollItemConfiguration configuration) : base(configuration)
        {
            DamagePercent = configuration.DamagePercent;
            damagedCode = GenerateDamagedCode(configuration.Code, configuration.DamagePercent);
        }

        public sealed override bool Use(IGameCore game)
        {
            if (RandomHelper.CheckChance(100 - DamagePercent))
                return base.Use(game);

            game.Journal.Write(new FailedToUseScrollMessage());
            game.Player.Damage(game.Journal, MagicDamageOnFailedScroll, Element.Magic);
            game.Journal.Write(new EnvironmentDamageMessage(game.Player, MagicDamageOnFailedScroll, Element.Magic));
            game.Map.GetCell(game.PlayerPosition).MagicEnergy.Disturbance += DisturbanceIncrementOnFailedScroll;
            return false;
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
    }

    public class AncientScrollItemConfiguration : ScrollItemConfiguration
    {
        public int DamagePercent { get; set; }
    }
}