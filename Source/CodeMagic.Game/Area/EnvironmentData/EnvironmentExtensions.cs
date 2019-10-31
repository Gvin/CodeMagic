using CodeMagic.Core.Area;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public static class EnvironmentExtensions
    {
        public static IGameEnvironment Cast(this IEnvironment environment)
        {
            return (IGameEnvironment)environment;
        }

        public static int Temperature(this IEnvironment environment)
        {
            return environment.Cast().Temperature;
        }

        public static int Pressure(this IEnvironment environment)
        {
            return environment.Cast().Pressure;
        }

        public static int MagicEnergyLevel(this IEnvironment environment)
        {
            return environment.Cast().MagicEnergyLevel;
        }

        public static int MagicDisturbanceLevel(this IEnvironment environment)
        {
            return environment.Cast().MagicDisturbanceLevel;
        }

        public static int MaxMagicEnergyLevel(this IEnvironment environment)
        {
            return environment.Cast().MaxMagicEnergyLevel;
        }
    }

    public static class AreaMapCellExtensions
    {
        public static int Temperature(this IAreaMapCell cell)
        {
            return cell.Environment.Temperature();
        }

        public static int Pressure(this IAreaMapCell cell)
        {
            return cell.Environment.Pressure();
        }

        public static int MagicEnergyLevel(this IAreaMapCell cell)
        {
            return cell.Environment.MagicEnergyLevel();
        }

        public static int MagicDisturbanceLevel(this IAreaMapCell cell)
        {
            return cell.Environment.MagicDisturbanceLevel();
        }

        public static int MaxMagicEnergyLevel(this IAreaMapCell cell)
        {
            return cell.Environment.MaxMagicEnergyLevel();
        }
    }
}