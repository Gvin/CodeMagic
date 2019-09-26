using System;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Configuration.Buildings
{
    public interface IBuildingsConfiguration
    {
        IBuildingConfiguration[] Buildings { get; }
    }

    public interface IBuildingConfiguration
    {
        Type Type { get; }

        string Name { get; }

        ItemRareness Rareness { get; }

        string Id { get; }

        string[] Unlocks { get; }

        bool AutoUnlock { get; }

        int BuildTime { get; }

        IBuildingMaterialConfiguration[] Cost { get; }
    }

    public interface IBuildingMaterialConfiguration
    {
        Type Type { get; }

        string Name { get; }

        int Count { get; }
    }
}