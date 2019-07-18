namespace CodeMagic.Core.Objects
{
    public interface IMapObject
    {
        string Name { get; }

        bool BlocksMovement { get; }

        bool BlocksProjectiles { get; }

        bool IsVisible { get; }

        bool BlocksVisibility { get; }

        bool BlocksEnvironment { get; }

        ZIndex ZIndex { get; }
    }
}