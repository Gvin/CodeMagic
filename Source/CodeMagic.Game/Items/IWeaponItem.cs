using System.Collections.Generic;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.Items
{
    public interface IWeaponItem : IHoldableItem
    {
        Dictionary<Element, int> MinDamage { get; }

        Dictionary<Element, int> MaxDamage { get; }

        int Accuracy { get; }

        Dictionary<Element, int> GenerateDamage();
    }
}