using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.SolidObjects
{
    public class EnergyWall : IMapObject, IDynamicObject
    {
        public EnergyWall(EnergyWallConfiguration configuration)
        {
            Name = configuration.Name;
            EnergyLeft = configuration.LifeTime;
        }

        public void Update(IGameCore game, Point position)
        {
            EnergyLeft--;
            if (EnergyLeft > 0)
                return;

            var cell = game.Map.GetCell(position);
            cell.Objects.Remove(this);
        }

        public int EnergyLeft { get; private set; }

        public bool Updated { get; set; }

        public string Name { get; }
        public bool BlocksMovement => true;
        public bool BlocksProjectiles => true;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => true;

        public ZIndex ZIndex => ZIndex.Wall;
    }

    public class EnergyWallConfiguration
    {
        public string Name { get; set; }

        public int LifeTime { get; set; }
    }
}