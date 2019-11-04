using CodeMagic.Core.Game.Locations;

namespace CodeMagic.Game.Locations
{
    public interface IGameLocation : ILocation
    {
        bool CanBuild { get; }

        bool CanCast { get; }

        bool CanFight { get; }

        bool CanUse { get; }
    }

    public static class GameLocationExtensions
    {
        public static IGameLocation Cast(this ILocation location)
        {
            return (IGameLocation) location;
        }

        public static bool CanBuild(this ILocation location)
        {
            return location.Cast().CanBuild;
        }

        public static bool CanCast(this ILocation location)
        {
            return location.Cast().CanCast;
        }

        public static bool CanFight(this ILocation location)
        {
            return location.Cast().CanFight;
        }

        public static bool CanUse(this ILocation location)
        {
            return location.Cast().CanUse;
        }
    }
}