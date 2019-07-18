using System.Linq;
using CodeMagic.Core.Configuration.Liquids;

namespace CodeMagic.Core.Configuration
{
    public static class ConfigurationManager
    {
        public static void InitializeConfiguration(IConfigurationProvider newProvider)
        {
            Current = newProvider;
        }

        public static IConfigurationProvider Current { get; private set; }

        public static ISpellConfiguration GetSpellConfiguration(string type)
        {
            return Current.Spells.SpellsConfiguration.FirstOrDefault(
                config => string.Equals(config.SpellType.ToLower(), type));
        }

        public static ILiquidConfiguration GetLiquidConfiguration(string type)
        {
            return Current.Liquids.LiquidsConfigurations.FirstOrDefault(
                config => string.Equals(config.Type.ToLower(), type));
        }
    }
}