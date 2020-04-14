using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public class TransformWaterSpellAction : SpellActionBase
    {
        private const string LiquidNameAcid = "acid";
        private const string LiquidNameOil = "oil";

        public const string ActionType = "transform_water";
        private readonly string result;
        private readonly int volume;

        public TransformWaterSpellAction(dynamic actionData) 
            : base(ActionType)
        {
            result = (string) actionData.result;
            volume = (int) actionData.volume;
        }

        public override Point Perform(Point position)
        {
            var targetCell = CurrentGame.Map.TryGetCell(position);
            if (targetCell == null)
                return position;

            var waterVolume = targetCell.GetVolume<WaterLiquid>();
            if (waterVolume <= 0)
                return position;

            var transmutingVolume = Math.Min(waterVolume, volume);

            targetCell.RemoveVolume<WaterLiquid>(transmutingVolume);
            CurrentGame.Map.AddObject(position, CreateTargetLiquid(transmutingVolume));

            return position;
        }

        private ILiquid CreateTargetLiquid(int targetVolume)
        {
            switch (result.ToLower())
            {
                case LiquidNameAcid:
                    return new AcidLiquid(targetVolume);
                case LiquidNameOil:
                    return new OilLiquid(targetVolume);
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
            return string.Equals(liquidName, LiquidNameAcid) ||
                   string.Equals(liquidName, LiquidNameOil);
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