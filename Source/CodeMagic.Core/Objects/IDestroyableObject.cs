using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects
{
    public interface IDestroyableObject : IMapObject
    {
        string Id { get; }

        int Health { get; set; }

        int MaxHealth { get; }

        void OnDeath(IAreaMap map, IJournal journal, Point position);

        void Damage(IJournal journal, int value, Element element);

        IObjectStatusesCollection Statuses { get; }

        double GetSelfExtinguishChance();

        List<IObjectEffect> ObjectEffects { get; }

        void ClearDamageRecords();
    }
}