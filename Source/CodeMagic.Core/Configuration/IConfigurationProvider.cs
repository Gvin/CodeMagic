using CodeMagic.Core.Configuration.Buildings;
using CodeMagic.Core.Configuration.Liquids;
using CodeMagic.Core.Configuration.Monsters;
using CodeMagic.Core.Configuration.Spells;

namespace CodeMagic.Core.Configuration
{
    public interface IConfigurationProvider
    {
        ISpellsConfiguration Spells { get; }

        IPhysicsConfiguration Physics { get; }

        ILiquidsConfiguration Liquids { get; }

        IMonstersConfiguration Monsters { get; }

        IBuildingsConfiguration Buildings { get; }
    }
}