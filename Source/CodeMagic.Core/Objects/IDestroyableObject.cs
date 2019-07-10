using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public interface IDestroyableObject : IMapObject
    {
        string Id { get; }

        string Name { get; }

        int Health { get; set; }

        int MaxHealth { get; set; }

        void OnDeath(IAreaMap map, Point position);

        void Damage(int value, Element? element = null);

        ObjectStatusesCollection Statuses { get; }

        int SelfExtinguishChance { get; }
    }
}