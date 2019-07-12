namespace CodeMagic.Core.Configuration
{
    public interface ITemperatureConfiguration
    {
        int NormalValue { get; }

        int MinValue { get; }

        int MaxValue { get; }
        
        int MaxTransferValue { get; }

        double TransferValueToDifferenceMultiplier { get; }

        int NormalizeSpeed { get; }

        ITemperatureDamageConfiguration[] ColdDamageConfiguration { get; }

        ITemperatureDamageConfiguration[] HeatDamageConfiguration { get; }
    }
}