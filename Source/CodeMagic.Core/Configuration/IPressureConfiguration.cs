namespace CodeMagic.Core.Configuration
{
    public interface IPressureConfiguration
    {
        int MinValue { get; }

        int MaxValue { get; }

        int NormalValue { get; }

        int NormalizeSpeed { get; }

        IPressureDamageConfiguration[] LowPressureDamageConfiguration { get; }

        IPressureDamageConfiguration[] HighPressureDamageConfiguration { get; }

        IPressureDamageConfiguration[] ChangePressureDamageConfiguration { get; }
    }
}