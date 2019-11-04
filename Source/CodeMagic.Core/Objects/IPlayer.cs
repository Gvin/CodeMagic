using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects
{
    public interface IPlayer : ICreatureObject
    {
        int Mana { get; set; }

        int MaxMana { get; set; }

        int HitChance { get; }

        int HungerPercent { get; set; }

        int MaxCarryWeight { get; }

        int ManaRegeneration { get; set; }

        Inventory Inventory { get; }

        int MaxVisibilityRange { get; }

        event EventHandler Died;
    }
}