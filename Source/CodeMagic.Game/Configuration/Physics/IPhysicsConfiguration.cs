namespace CodeMagic.Game.Configuration.Physics
{
    public interface IPhysicsConfiguration
    {
        ITemperatureConfiguration TemperatureConfiguration { get; }

        IPressureConfiguration PressureConfiguration { get; }

        IMagicEnergyConfiguration MagicEnergyConfiguration { get; }
    }
}