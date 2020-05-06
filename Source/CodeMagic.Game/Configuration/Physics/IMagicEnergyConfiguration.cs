namespace CodeMagic.Game.Configuration.Physics
{
    public interface IMagicEnergyConfiguration
    {
        int MaxValue { get; }

        int MaxTransferValue { get; }

        int RegenerationValue { get; }

        int DisturbanceStartLevel { get; }

        int DisturbanceDamageStartLevel { get; }

        int DisturbanceIncrement { get; }
    }
}