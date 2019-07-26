namespace CodeMagic.Core.Configuration
{
    public interface IPressureDamageConfiguration
    {
        int PressureLevel { get; }

        double DamageMultiplier { get; }
    }
}