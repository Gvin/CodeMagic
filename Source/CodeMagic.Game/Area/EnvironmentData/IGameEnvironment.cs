using CodeMagic.Core.Area;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public interface IGameEnvironment : IEnvironment
    {
        int Temperature { get; set; }

        int Pressure { get; set; }

        int MagicEnergyLevel { get; set; }

        int MaxMagicEnergyLevel { get; }

        int MagicDisturbanceLevel { get; set; }
    }
}