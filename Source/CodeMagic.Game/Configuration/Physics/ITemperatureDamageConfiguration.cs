namespace CodeMagic.Game.Configuration.Physics
{
    public interface ITemperatureDamageConfiguration
    {
        int TemperatureLevel { get; }

        double DamageMultiplier { get; }
    }
}