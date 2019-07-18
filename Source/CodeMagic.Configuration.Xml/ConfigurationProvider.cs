using System.IO;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Liquids;
using CodeMagic.Configuration.Xml.Types.Physics;
using CodeMagic.Configuration.Xml.Types.Spells;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Liquids;

namespace CodeMagic.Configuration.Xml
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private ConfigurationProvider(
            ISpellsConfiguration spells,
            IPhysicsConfiguration physics,
            ILiquidsConfiguration liquids)
        {
            Spells = spells;
            Physics = physics;
            Liquids = liquids;
        }

        public ISpellsConfiguration Spells { get; }

        public IPhysicsConfiguration Physics { get; }

        public ILiquidsConfiguration Liquids { get; }

        public static IConfigurationProvider Load(Stream spellsConfigStream, Stream physicsConfigStream, Stream liquidsConfigStream)
        {
            var spells = LoadConfig<ISpellsConfiguration, XmlSpellsConfigurationType>(spellsConfigStream);
            var physics = LoadConfig<IPhysicsConfiguration, XmlPhysicsConfigurationType>(physicsConfigStream);
            var liquids = LoadConfig<ILiquidsConfiguration, XmlLiquidsConfiguration>(liquidsConfigStream);
            return new ConfigurationProvider(spells, physics, liquids);
        }

        private static TConfig LoadConfig<TConfig, TSerializable>(Stream fileStream)
        {
            var serializer = new XmlSerializer(typeof(TSerializable));
            var data = serializer.Deserialize(fileStream);
            if (data is TConfig)
            {
                return (TConfig) data;
            }

            return default(TConfig);
        }
    }
}