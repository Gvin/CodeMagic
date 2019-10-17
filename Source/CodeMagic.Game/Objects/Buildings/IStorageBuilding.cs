using System;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.Objects.Buildings
{
    public interface IStorageBuilding
    {
        event EventHandler Opened;

        Inventory Inventory { get; }

        string Name { get; }

        int MaxWeight { get; }
    }
}