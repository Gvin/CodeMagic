using System;
using System.Linq;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Configuration.Spells;

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
            var result = Current.Spells.SpellsConfiguration.FirstOrDefault(
                config => string.Equals(config.SpellType.ToLower(), type));
            if (result == null)
                throw new ApplicationException($"Configuration for spell action \"{type}\" not found.");

            return result;
        }

        public static ILiquidConfiguration GetLiquidConfiguration(string type)
        {
            var result = Current.Liquids.LiquidsConfigurations.FirstOrDefault(
                config => string.Equals(config.Type.ToLower(), type));
            if (result == null)
                throw new ApplicationException($"Configuration for liquid \"{type}\" not found.");

            return result;
        }
    }
}