using CodeMagic.Core.Area;

namespace CodeMagic.Core.Objects
{
    public interface IFireSpreadingObject
    {
        bool GetIsOnFire(AreaMapCell cell);

        bool SpreadsFire { get; }
    }
}