using System.Collections.Generic;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items.Bonuses.Armor
{
    public interface IArmorItemBonus : IItemBonus
    {
        Element Element { get; }

        int Protection { get; }
    }
}