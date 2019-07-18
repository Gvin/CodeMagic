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

        int SteamPressureMultiplier { get; }

        ILiquidConfigurationCustomValue[] CustomValues { get; }
    }

    public interface ILiquidConfigurationCustomValue
    {
        string Key { get; }

        string Value { get; }
    }
}