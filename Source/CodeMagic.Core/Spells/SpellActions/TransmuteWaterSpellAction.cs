using System;
using System.Collections.Generic;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class TransmuteWaterSpellAction : SpellActionBase
    {
        private const string LiquidNameAcid = "acid";

        public const string ActionType = "transmute_water";
        private readonly string result;
        private readonly int volume;

        public TransmuteWaterSpellAction(dynamic actionData) 
            : base(ActionType)
        {
            result = (string) actionData.result;
            volume = (int) actionData.volume;
        }

        public override Point Perform(IGameCore game, Point position)
        {
            if (!game.Map.ContainsCell(position))
                return position;

            var targetCell = game.Map.GetCell(position);
            var waterVolume = targetCell.Liquids.GetLiquidVolume<WaterLiquid>();
            var transmutingVolume = Math.Min(waterVolume, volume);

            targetCell.Liquids.RemoveLiquid<WaterLiquid>(transmutingVolume);
            targetCell.Liquids.AddLiquid(CreateTargetLiquid(transmutingVolume));

            return position;
        }

        private ILiquid CreateTargetLiquid(int targetVolume)
        {
            switch (result.ToLower())
            {
                case LiquidNameAcid:
                    return new AcidLiquid(targetVolume);
                default:
                    throw new SpellException($"Unknown liquid result: {result}");
            }
        }

        public override int ManaCost => GetManaCost(volume);

        public override JsonData GetJson()
        {
            return GetJson(result, volume);
        }

        private static bool CheckKnownLiquid(string result)
        {
            var liquidName = result.ToLower();
            return string.Equals(liquidName, LiquidNameAcid);
        }

        public static JsonData GetJson(string result, int volume)
        {
            var knownLiquid = CheckKnownLiquid(result);
            if (!knownLiquid)
                throw new SpellException($"Unknown liquid result: {result}");
            if (volume <= 0)
                throw new SpellException("Volume should be greater than 0.");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"result", result},
                {"volume", volume},
                {"manaCost", GetManaCost(ActionType, volume)}
            });
        }
    }
}