using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Implementations.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public abstract class MinableWall : DestroyableObject, IWorldImageProvider, IPlaceConnectionObject, IResourceObject
    {
        private const int MinResourceCount = 0;
        private const int MaxResourceCount = 5;
        private const int PickaxeDamageMultiplier = 2;
        private readonly List<Point> connectedTiles;

        protected MinableWall(string name, int health) 
            : base(new DestroyableObjectConfiguration
            {
                Name = name,
                BaseProtection = new Dictionary<Element, int>
                {
                    {Element.Blunt, 90},
                    {Element.Electricity, 100},
                    {Element.Fire, 100},
                    {Element.Frost, 100},
                    {Element.Magic, 100},
                    {Element.Piercing, 100},
                    {Element.Slashing, 90}
                },
                ZIndex = ZIndex.Wall,
                Size = ObjectSize.Huge,
                Health = health,
                MaxHealth = health,
                CatchFireChanceMultiplier = 0,
                SelfExtinguishChance = 100
            })
        {
            connectedTiles = new List<Point>();
        }

        public override bool BlocksEnvironment => true;

        public override bool BlocksMovement => true;

        public override bool BlocksProjectiles => true;

        public override bool BlocksVisibility => true;

        protected bool HasConnectedTile(int relativeX, int relativeY)
        {
            return connectedTiles.Any(point => point.X == relativeX && point.Y == relativeY);
        }

        public void OnPlaced(IAreaMap map, Point position)
        {
            CheckWallInDirection(map, position, -1, -1); // Top Left
            CheckWallInDirection(map, position, 0, -1); // Top
            CheckWallInDirection(map, position, 1, -1); // Top Right

            CheckWallInDirection(map, position, -1, 0); // Left
            CheckWallInDirection(map, position, 1, 0); // Right

            CheckWallInDirection(map, position, 1, 1); // Bottom Left
            CheckWallInDirection(map, position, 0, 1); // Bottom
            CheckWallInDirection(map, position, 1, 1); // Bottom Right
        }

        public void AddConnectedTile(Point position)
        {
            connectedTiles.Add(position);
        }

        private void CheckWallInDirection(IAreaMap map, Point position, int relativeX, int relativeY)
        {
            var wallUp = GetWall(map, position, relativeX, relativeY);
            if (wallUp != null)
            {
                connectedTiles.Add(new Point(relativeX, relativeY));
                wallUp.AddConnectedTile(new Point(relativeX * (-1), relativeY * (-1)));
            }
        }

        private IPlaceConnectionObject GetWall(IAreaMap map, Point position, int relativeX, int relativeY)
        {
            var nearPosition = new Point(position.X + relativeX, position.Y + relativeY);
            var cell = map.TryGetCell(nearPosition);
            return cell?.Objects.OfType<IPlaceConnectionObject>().FirstOrDefault(CanConnectTo);
        }

        protected abstract bool CanConnectTo(IMapObject obj);

        protected abstract IItem CreateResource();

        public abstract SymbolsImage GetWorldImage(IImagesStorage storage);

        public void UseTool(IGameCore game, WeaponItem weapon, int damage, Element element)
        {
            if (weapon is Pickaxe axe)
            {
                var realDamage = damage * PickaxeDamageMultiplier;
                if (RandomHelper.CheckChance(axe.PickaxePower))
                {
                    game.Player.Inventory.AddItem(CreateResource());
                }
                ApplyRealDamage(realDamage);
                return;
            }
            Damage(game.Journal, damage, element);
        }

        public override void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
            base.OnDeath(map, journal, position);

            var woodCount = RandomHelper.GetRandomValue(MinResourceCount, MaxResourceCount);
            for (var counter = 0; counter < woodCount; counter++)
            {
                map.AddObject(position, CreateResource());
            }
        }
    }
}