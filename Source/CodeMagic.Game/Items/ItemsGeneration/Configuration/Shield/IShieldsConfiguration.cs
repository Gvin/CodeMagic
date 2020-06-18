using CodeMagic.Game.Items.ItemsGeneration.Configuration.Description;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield
{
    public interface IShieldsConfiguration
    {
        public IShieldConfiguration SmallShieldConfiguration { get; }

        public IShieldConfiguration MediumShieldConfiguration { get; }

        public IShieldConfiguration BigShieldConfiguration { get; }

        IDescriptionConfiguration DescriptionConfiguration { get; }
    }
}