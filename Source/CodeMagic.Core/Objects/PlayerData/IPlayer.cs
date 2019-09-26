using System;
using CodeMagic.Core.Configuration.Buildings;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects.PlayerData
{
    public interface IPlayer : ICreatureObject
    {
        int Mana { get; set; }

        int MaxMana { get; set; }

        int HitChance { get; }

        int MaxCarryWeight { get; }

        int ManaRegeneration { get; set; }

        Inventory Inventory { get; }

        Equipment Equipment { get; }

        int MaxVisibilityRange { get; }

        event EventHandler Died;

        bool GetIfBuildingUnlocked(IBuildingConfiguration building);

        bool UnlockBuilding(IBuildingConfiguration building);
    }
}