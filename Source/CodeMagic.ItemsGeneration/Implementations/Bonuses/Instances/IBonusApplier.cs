using CodeMagic.Core.Items;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;

namespace CodeMagic.ItemsGeneration.Implementations.Bonuses.Instances
{
    internal interface IBonusApplier
    {
        void Apply(IBonusConfiguration config, ItemConfiguration itemConfiguration, NameBuilder name);
    }
}