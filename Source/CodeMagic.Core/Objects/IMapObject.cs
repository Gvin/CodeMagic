using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Objects
{
    public interface IMapObject : ISaveable
    {
        string Name { get; }

        bool BlocksMovement { get; }

        bool BlocksProjectiles { get; }

        bool IsVisible { get; }

        bool BlocksVisibility { get; }

        bool BlocksAttack { get; }

        bool BlocksEnvironment { get; }

        ZIndex ZIndex { get; }

        ObjectSize Size { get; }

        bool Equals(IMapObject other);
    }
}