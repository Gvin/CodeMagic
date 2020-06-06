﻿namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor
{
    public interface IArmorPieceConfiguration
    {
        string TypeName { get; }

        ArmorClass Class { get; }

        string[] Images { get; }

        string EquippedImage { get; }

        IArmorRarenessConfiguration[] RarenessConfigurations { get; }

        IWeightConfiguration[] Weight { get; }
    }
}