namespace CodeMagic.Game.Configuration.Physics
{
    public interface IPressureDamageConfiguration
    {
        int PressureLevel { get; }

        double DamageMultiplier { get; }
    }
}