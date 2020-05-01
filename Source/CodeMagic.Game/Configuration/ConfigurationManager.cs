﻿using System;
using System.Linq;
using CodeMagic.Game.Configuration.Levels;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.Configuration.Spells;
using CodeMagic.Game.Configuration.Treasure;

namespace CodeMagic.Game.Configuration
{
    public static class ConfigurationManager
    {
        public static void InitializeConfiguration(IConfigurationProvider newProvider)
        {
            Current = newProvider;
        }

        public static IConfigurationProvider Current { get; private set; }

        public static ISpellConfiguration GetSpellConfiguration(string type)
        {
            var result = Current.Spells.SpellsConfiguration.FirstOrDefault(
                config => string.Equals(config.SpellType.ToLower(), type.ToLower()));
            if (result == null)
                throw new ApplicationException($"Configuration for spell action \"{type}\" not found.");

            return result;
        }

        public static ILiquidConfiguration GetLiquidConfiguration(string type)
        {
            var result = Current.Liquids.LiquidsConfigurations.FirstOrDefault(
                config => string.Equals(config.Type.ToLower(), type.ToLower()));
            if (result == null)
                throw new ApplicationException($"Configuration for liquid \"{type}\" not found.");

            return result;
        }
    }

    public interface IConfigurationProvider
    {
        IPhysicsConfiguration Physics { get; }

        ILiquidsConfiguration Liquids { get; }

        ISpellsConfiguration Spells { get; }

        IMonstersConfiguration Monsters { get; }

        ILevelsConfiguration Levels { get; }

        ITreasureConfiguration Treasure { get; }
    }
}