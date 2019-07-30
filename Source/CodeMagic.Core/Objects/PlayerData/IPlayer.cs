using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects.PlayerData
{
    public interface IPlayer : ICreatureObject
    {
        int Mana { get; set; }

        int MaxMana { get; set; }

        int ManaRegeneration { get; set; }

        Inventory Inventory { get; }

        Equipment Equipment { get; }

        int MaxVisibilityRange { get; }

        event EventHandler Died;
    }
}