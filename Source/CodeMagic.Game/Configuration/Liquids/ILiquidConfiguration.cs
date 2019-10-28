namespace CodeMagic.Core.Configuration.Liquids
{
    public interface ILiquidConfiguration
    {
        string Type { get; }

        int FreezingPoint { get; }

        int BoilingPoint { get; }

        int MinVolumeForEffect { get; }

        int MaxVolumeBeforeSpread { get; }

        int MaxSpreadVolume { get; }

        int EvaporationMultiplier { get; }

        double EvaporationTemperatureMultiplier { get; }

        double CondensationTemperatureMultiplier { get; }

        double FreezingTemperatureMultiplier { get; }

        double MeltingTemperatureMultiplier { get; }

        ISteamConfiguration Steam { get; }

        ILiquidConfigurationCustomValue[] CustomValues { get; }
    }

    public interface ISteamConfiguration
    {
        int PressureMultiplier { get; }

        int VolumeMultiplier { get; }

        double ThicknessMultiplier { get; }

        int MaxVolumeBeforeSpread { get; }

        int MaxSpreadVolume { get; }
    }

    public interface ILiquidConfigurationCustomValue
    {
        string Key { get; }

        string Value { get; }
    }
}