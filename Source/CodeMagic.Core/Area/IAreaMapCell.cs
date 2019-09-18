using CodeMagic.Core.Area.EnvironmentData;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.LiquidObjects;

namespace CodeMagic.Core.Area
{
    public interface IAreaMapCell
    {
        CellLightData LightLevel { get; }

        IMapObject[] Objects { get; }

        int GetVolume<T>() where T : ILiquidObject;

        void RemoveVolume<T>(int volume) where T : ILiquidObject;

        bool BlocksMovement { get; }

        bool BlocksEnvironment { get; }

        bool BlocksVisibility { get; }

        bool BlocksProjectiles { get; }

        bool HasSolidObjects { get; }

        IDestroyableObject GetBiggestDestroyable();

        int Temperature { get; set; }

        int Pressure { get; set; }

        int MagicEnergyLevel { get; set; }

        int MagicDisturbanceLevel { get; set; }
    }
}