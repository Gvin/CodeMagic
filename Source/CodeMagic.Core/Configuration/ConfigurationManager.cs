using System.Linq;

namespace CodeMagic.Core.Configuration
{
    public static class ConfigurationManager
    {
        private static IConfigurationProvider provider;

        public static void InitializeConfiguration(IConfigurationProvider newProvider)
        {
            provider = newProvider;
        }

        public static IConfigurationProvider Current => provider;

        public static ISpellConfiguration GetSpellConfiguration(string type)
        {
            return provider.SpellsConfiguration.FirstOrDefault(
                config => string.Equals(config.SpellType.ToLower(), type));
        }
    }
}