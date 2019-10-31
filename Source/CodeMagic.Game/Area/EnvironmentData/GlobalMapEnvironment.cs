using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public class GlobalMapEnvironment : IGameEnvironment
    {
        private readonly IPhysicsConfiguration configuration;

        public GlobalMapEnvironment(IPhysicsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Update(IAreaMap map, Point position, IAreaMapCell cell, IJournal journal)
        {
            // Do nothing.
        }

        public void Balance(IAreaMapCell cell, IAreaMapCell otherCell)
        {
            // Do nothing.
        }

        public int Temperature
        {
            get => configuration.TemperatureConfiguration.NormalValue;
            set { }
        }
        public int Pressure
        {
            get => configuration.PressureConfiguration.NormalValue;
            set { }
        }

        public int MagicEnergyLevel
        {
            get => int.MaxValue;
            set { }
        }

        public int MaxMagicEnergyLevel => int.MaxValue;

        public int MagicDisturbanceLevel
        {
            get => 0;
            set { }
        }
    }
}