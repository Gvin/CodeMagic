using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal class ProtectionBonusApplier : IBonusApplier
    {
        public const string BonusType = "ProtectionBonus";
        private const string NamePostfixTemplate = "{0} Protection";
        private const string KeyMax = "Max";
        private const string KeyMin = "Min";

        public void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name)
        {
            var armorConfig = (ArmorItemConfiguration) itemConfiguration;

            var element = ItemGeneratorHelper.GetRandomDamageElement();
            var min = int.Parse(config.Values[KeyMin]);
            var max = int.Parse(config.Values[KeyMax]);

            var protection = RandomHelper.GetRandomValue(min, max);

            if (armorConfig.Protection.ContainsKey(element))
            {
                armorConfig.Protection[element] += protection;
            }
            else
            {
                armorConfig.Protection.Add(element, protection);
            }

            var bonusCode = GetBonusCode(element);
            name.AddNamePostfix(bonusCode, string.Format(NamePostfixTemplate, TextHelper.GetElementName(element)));
            name.AddDescription(bonusCode, GetBonusDescription(element));
        }

        private static string GetBonusCode(Element element)
        {
            return $"protection_bonus_{element}";
        }

        private static string GetBonusDescription(Element element)
        {
            var elementName = TextHelper.GetElementName(element);
            return $"It protects you from {elementName} better that similar items.";
        }
    }
}