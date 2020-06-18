using System;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects
{
    public interface IPlayer : ICreatureObject
    {
        int Mana { get; set; }

        int MaxMana { get; }

        int HungerPercent { get; set; }

        int MaxCarryWeight { get; }

        int ManaRegeneration { get; }

        Inventory Inventory { get; }

        int MaxVisibilityRange { get; }

        event EventHandler Died;

        int Experience { get; }

        int Level { get; }

        void AddExperience(int exp);
    }
}