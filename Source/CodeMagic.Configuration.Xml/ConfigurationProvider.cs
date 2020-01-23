using System.IO;
using System.Xml.Serialization;
using CodeMagic.Configuration.Xml.Types.Items;
using CodeMagic.Configuration.Xml.Types.Liquids;
using CodeMagic.Configuration.Xml.Types.Monsters;
using CodeMagic.Configuration.Xml.Types.Physics;
using CodeMagic.Configuration.Xml.Types.Spells;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.Configuration.Spells;
using CodeMagic.Game.Items.ItemsGeneration.Configuration;

namespace CodeMagic.Configuration.Xml
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private ConfigurationProvider(
            ISpellsConfiguration spells,
            IPhysicsConfiguration physics,
            ILiquidsConfiguration liquids,
            IItemGeneratorConfiguration itemGenerator,
            IMonstersConfiguration monsters)
        {
            Spells = spells;
            Physics = physics;
            Liquids = liquids;
            ItemGenerator = itemGenerator;
            Monsters = monsters;
        }

        public ISpellsConfiguration Spells { get; }

        public IPhysicsConfiguration Physics { get; }

        public ILiquidsConfiguration Liquids { get; }

        public IItemGeneratorConfiguration ItemGenerator { get; }

        public IMonstersConfiguration Monsters { get; }

        public static IConfigurationProvider Load(
            Stream spellsConfigStream,
            Stream physicsConfigStream,
            Stream liquidsConfigStream,
            Stream itemsGeneratorConfigStream,
            Stream monstersConfigStream)
        {
            var spells = LoadConfig<ISpellsConfiguration, XmlSpellsConfigurationType>(spellsConfigStream);
            var physics = LoadConfig<IPhysicsConfiguration, XmlPhysicsConfigurationType>(physicsConfigStream);
            var liquids = LoadConfig<ILiquidsConfiguration, XmlLiquidsConfiguration>(liquidsConfigStream);
            var monsters = LoadConfig<IMonstersConfiguration, XmlMonstersConfiguration>(monstersConfigStream);

            var itemGenerator = LoadConfig<IItemGeneratorConfiguration, XmlItemGeneratorConfiguration>(itemsGeneratorConfigStream);

            return new ConfigurationProvider(spells, physics, liquids, itemGenerator, monsters);
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