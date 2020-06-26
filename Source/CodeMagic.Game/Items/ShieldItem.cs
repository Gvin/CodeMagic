using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public class ShieldItem : HoldableDurableItemBase
    {
        private const string SaveKeyBlocksDamage = "BlocksDamage";
        private const string SaveKeyProtectChance = "ProtectChance";
        private const string SaveKeyHitChancePenalty = "HitChancePenalty";

        public ShieldItem(SaveData data) : base(data)
        {
            BlocksDamage = data.GetIntValue(SaveKeyBlocksDamage);
            HitChancePenalty = data.GetIntValue(SaveKeyHitChancePenalty);
            ProtectChance = data.GetIntValue(SaveKeyProtectChance);
        }

        public ShieldItem(ShieldItemConfiguration configuration) : base(configuration)
        {
            BlocksDamage = configuration.BlocksDamage;
            ProtectChance = configuration.ProtectChance;
            HitChancePenalty = configuration.HitChancePenalty;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();

            data.Add(SaveKeyBlocksDamage, BlocksDamage);
            data.Add(SaveKeyHitChancePenalty, HitChancePenalty);
            data.Add(SaveKeyProtectChance, ProtectChance);

            return data;
        }

        public int BlocksDamage { get; }

        public int ProtectChance { get; }

        public int HitChancePenalty { get; }

        public override StyledLine[] GetDescription(Player player)
        {
            var result = GetCharacteristicsDescription(player).ToList();

            result.Add(StyledLine.Empty);
            result.AddRange(TextHelper.ConvertDescription(Description));

            return result.ToArray();
        }

        private StyledLine[] GetCharacteristicsDescription(Player player)
        {
            var rightHandShield = player.Equipment.RightHandItem as ShieldItem;
            var leftHandShield = player.Equipment.LeftHandItem as ShieldItem;

            var result = new List<StyledLine>();

            if (Equals(leftHandShield) || Equals(rightHandShield) || leftHandShield == null)
            {
                result.Add(TextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(TextHelper.GetCompareWeightLine(Weight, leftHandShield.Weight));
            }

            result.Add(StyledLine.Empty);

            result.Add(TextHelper.GetDurabilityLine(Durability, MaxDurability));

            result.Add(StyledLine.Empty);

            AddProtectionDescription(result, leftHandShield, rightHandShield);

            result.Add(StyledLine.Empty);

            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                TextHelper.AddBonusesDescription(this, null, result);
            }
            else
            {
                TextHelper.AddBonusesDescription(this, leftHandShield, result);
            }
            result.Add(StyledLine.Empty);

            TextHelper.AddLightBonusDescription(this, result);

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledLine> descr, ShieldItem leftHandShield,
            ShieldItem rightHandShield)
        {
            var hitChanceLine = new StyledLine { "Protect Chance: " };
            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                hitChanceLine.Add(TextHelper.GetValueString(ProtectChance, "%", false));
            }
            else
            {
                hitChanceLine.Add(TextHelper.GetCompareValueString(ProtectChance, leftHandShield.ProtectChance, "%", false));
            }
            descr.Add(hitChanceLine);

            var blocksDamageLine = new StyledLine { "Blocks Damage: " };
            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                blocksDamageLine.Add(TextHelper.GetValueString(BlocksDamage, formatBonus: false));
            }
            else
            {
                blocksDamageLine.Add(TextHelper.GetCompareValueString(BlocksDamage, leftHandShield.BlocksDamage, formatBonus: false));
            }
            descr.Add(blocksDamageLine);

            var hitChancePenaltyLine = new StyledLine { "Hit Chance Penalty: " };
            if (Equals(rightHandShield) || Equals(leftHandShield) || leftHandShield == null)
            {
                hitChancePenaltyLine.Add(TextHelper.GetValueString(HitChancePenalty, "%", false));
            }
            else
            {
                hitChancePenaltyLine.Add(TextHelper.GetCompareValueString(HitChancePenalty, leftHandShield.HitChancePenalty, "%", false));
            }
            descr.Add(hitChancePenaltyLine);
        }
    }

    public class ShieldItemConfiguration : HoldableItemConfiguration
    {
        public int BlocksDamage { get; set; }

        public int ProtectChance { get; set; }

        public int HitChancePenalty { get; set; }
    }
}