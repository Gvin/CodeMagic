namespace CodeMagic.Core.Configuration
{
    public interface IConfigurationProvider
    {
        ISpellConfiguration[] SpellsConfiguration { get; }

        ITemperatureConfiguration TemperatureConfiguration { get; }

        IPressureConfiguration PressureConfiguration { get; }
    }
}