namespace CodeMagic.Core.Objects
{
    public interface IMapObject
    {
        string Name { get; }

        bool BlocksMovement { get; }

        bool BlocksProjectiles { get; }

        bool IsVisible { get; }

        bool BlocksVisibility { get; }

        bool BlocksAttack { get; }

        bool BlocksEnvironment { get; }

        ZIndex ZIndex { get; }

        bool Equals(IMapObject other);

        ObjectSize Size { get; }
    }
}