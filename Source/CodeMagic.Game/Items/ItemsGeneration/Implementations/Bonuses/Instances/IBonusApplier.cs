using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal interface IBonusApplier
    {
        void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name);
    }
}