namespace CodeMagic.Core.Configuration
{
    public interface ITemperatureDamageConfiguration
    {
        int TemperatureLevel { get; }

        double DamageMultiplier { get; }
    }
}