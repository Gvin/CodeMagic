using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public interface IDestroyableObject : IMapObject
    {
        string Id { get; }

        int Health { get; set; }

        int MaxHealth { get; set; }

        void OnDeath(IAreaMap map, Point position);

        void Damage(int value, Element? element = null);

        ObjectStatusesCollection Statuses { get; }

        int GetSelfExtinguishChance();

        IDamageRecord[] DamageRecords { get; }

        void ClearDamageRecords();
    }

    public interface IDamageRecord : IInjectable
    {
        int Value { get; }

        Element? Element { get; }
    }
}