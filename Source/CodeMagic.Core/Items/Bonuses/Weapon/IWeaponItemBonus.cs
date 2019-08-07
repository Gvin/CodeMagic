using System.Collections.Generic;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items.Bonuses.Weapon
{
    public interface IWeaponItemBonus : IItemBonus
    {
        int MaxDamage { get; }

        int MinDamage { get; }

        Element Element { get; }
    }
}