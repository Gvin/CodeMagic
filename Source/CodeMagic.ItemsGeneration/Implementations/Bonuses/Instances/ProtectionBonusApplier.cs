using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations.Items;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.ItemsGeneration.Implementations.Bonuses.Instances
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

            name.Postfixes.Add(string.Format(NamePostfixTemplate, ItemTextHelper.GetElementName(element)));
        }
    }
}