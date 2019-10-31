using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects
{
    public interface IFireSpreadingObject
    {
        bool GetIsOnFire(IAreaMapCell cell);

        bool SpreadsFire { get; }

        int BurningTemperature { get; }
    }
}