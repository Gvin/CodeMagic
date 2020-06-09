using System;

namespace CodeMagic.Game.Items.Usable.Potions
{
    internal interface IPotionDataFactory
    {
        PotionData GetPotionData(PotionType type, PotionSize size);
    }

    internal class PotionDataFactory : IPotionDataFactory
    {
        public PotionData GetPotionData(PotionType type, PotionSize size)
        {
            switch (type)
            {
                case PotionType.Health:
                    return new HealthPotionData(size);
                case PotionType.Mana:
                    return new ManaPotionData(size);
                case PotionType.Restoration:
                    return new RestorationPotionData(size);
                case PotionType.Stamina:
                    return new StaminaPotionData(size);
                case PotionType.Paralyze:
                    return new ParalyzePotionData(size);
                case PotionType.Freeze:
                    return new FreezePotionData(size);
                case PotionType.Hunger:
                    return new HungerPotionData(size);
                case PotionType.Blind:
                    return new BlindPotionData(size);
                default:
                    throw new ArgumentException($"Unknown potion type: {type}", nameof(type));
            }
        }
    }
}