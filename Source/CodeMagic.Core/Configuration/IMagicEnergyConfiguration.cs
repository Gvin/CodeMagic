namespace CodeMagic.Core.Configuration
{
    public interface IMagicEnergyConfiguration
    {
        int MaxValue { get; }

        int MaxTransferValue { get; }

        int RegenerationValue { get; }

        int DisturbanceStartLevel { get; }

        double DisturbanceDamageMultiplier { get; }

        int DisturbanceDamageStartLevel { get; }

        int DisturbanceIncrement { get; }
    }
}