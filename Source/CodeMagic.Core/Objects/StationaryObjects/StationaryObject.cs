using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.DecorativeObjects;

namespace CodeMagic.Core.Objects.StationaryObjects
{
    public class StationaryObject : DestroyableObject
    {
        public StationaryObject(StationaryObjectConfiguration configuration) 
            : base(configuration)
        {
            Type = configuration.Type;
        }

        public override bool BlocksMovement => true;

        public string Type { get; }

        public override void OnDeath(IAreaMap map, Point position)
        {
            base.OnDeath(map, position);

            PlaceOnDeathParts(map, position);
        }

        private void PlaceOnDeathParts(IAreaMap map, Point position)
        {
            var onDeathParts = GetOnDeathParts();
            if (onDeathParts == null)
                return;

            map.GetCell(position).Objects.Add(GetOnDeathParts());
        }

        private IMapObject GetOnDeathParts()
        {
            switch (Type)
            {
                case StationaryObjectConfiguration.ObjectTypeCrates:
                    return new DecorativeObject(new DecorativeObjectConfiguration
                    {
                        IsBigObject = false,
                        Name = "Wood Pieces",
                        Type = DecorativeObjectConfiguration.ObjectTypeWoodPieces
                    });
                default:
                    return null;
            }
        }
    }
}