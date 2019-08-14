using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public interface IDestroyableObject : IMapObject
    {
        string Id { get; }

        int Health { get; set; }

        int MaxHealth { get; set; }

        void OnDeath(IGameCore game, Point position);

        void Damage(Journal journal, int value, Element element);

        ObjectStatusesCollection Statuses { get; }

        int GetSelfExtinguishChance();

        List<IObjectEffect> ObjectEffects { get; }

        void ClearDamageRecords();
    }
}