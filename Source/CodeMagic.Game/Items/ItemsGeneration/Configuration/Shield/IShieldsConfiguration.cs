using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield
{
    public interface IShieldsConfiguration
    {
        IShieldConfiguration SmallShieldConfiguration { get; }

        IShieldConfiguration MediumShieldConfiguration { get; }

        IShieldConfiguration BigShieldConfiguration { get; }

        IDescriptionConfiguration DescriptionConfiguration { get; }
    }
}