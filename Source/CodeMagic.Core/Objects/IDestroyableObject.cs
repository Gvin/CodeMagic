using System.Collections.Generic;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public interface IDestroyableObject : IMapObject
    {
        string Id { get; }

        int Health { get; set; }

        int MaxHealth { get; }

        int DodgeChance { get; }

        void OnDeath(Point position);

        void MeleDamage(Point position, Direction attackDirection, int damage, Element element);

        void Damage(Point position, int damage, Element element);

        IObjectStatusesCollection Statuses { get; }

        double GetSelfExtinguishChance();

        List<IObjectEffect> ObjectEffects { get; }

        void ClearDamageRecords();
    }
}