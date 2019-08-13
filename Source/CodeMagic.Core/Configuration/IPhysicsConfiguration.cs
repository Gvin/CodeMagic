namespace CodeMagic.Core.Configuration
{
    public interface IPhysicsConfiguration
    {
        ITemperatureConfiguration TemperatureConfiguration { get; }

        IPressureConfiguration PressureConfiguration { get; }

        IMagicEnergyConfiguration MagicEnergyConfiguration { get; }
    }
}