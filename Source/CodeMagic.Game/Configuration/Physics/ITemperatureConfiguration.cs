namespace CodeMagic.Game.Configuration.Physics
{
    public interface ITemperatureConfiguration
    {
        int NormalValue { get; }

        int MinValue { get; }

        int MaxValue { get; }
        
        int MaxTransferValue { get; }

        double TransferValueToDifferenceMultiplier { get; }

        int NormalizeSpeedInside { get; }

        int NormalizeSpeedOutside { get; }

        ITemperatureDamageConfiguration ColdDamageConfiguration { get; }

        ITemperatureDamageConfiguration HeatDamageConfiguration { get; }
    }
}