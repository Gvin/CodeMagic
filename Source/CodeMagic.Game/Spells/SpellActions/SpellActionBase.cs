using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Configuration.Spells;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public abstract class SpellActionBase : ISpellAction
    {
        private readonly double manaCostMultiplier;
        private readonly int manaCostPower;
        private readonly string actionType;
        private readonly ISpellConfiguration configuration;

        protected SpellActionBase(string actionType)
        {
            this.actionType = actionType;
            configuration = ConfigurationManager.GetSpellConfiguration(actionType);
            if (configuration == null)
                throw new ApplicationException($"Configuration for spell action \"{actionType}\" not found.");

            manaCostMultiplier = configuration.ManaCostMultiplier;
            manaCostPower = configuration.ManaCostPower;
        }

        public abstract Point Perform(IAreaMap map, IJournal journal, Point position);

        public abstract int ManaCost { get; }

        protected int GetManaCost(int value)
        {
            return GetManaCost(value, manaCostMultiplier, manaCostPower);
        }

        private static int GetManaCost(int value, double multiplier, int power)
        {
            var basement = Math.Ceiling(value * multiplier);
            return (int) Math.Pow(basement, power);
        }

        protected static int GetManaCost(string actionType, int value)
        {
            var configuration = ConfigurationManager.GetSpellConfiguration(actionType);
            if (configuration == null)
                throw new ApplicationException($"Configuration for spell action \"{actionType}\" not found.");

            return GetManaCost(value, configuration.ManaCostMultiplier, configuration.ManaCostPower);
        }

        protected string GetCustomValue(string key)
        {
            var value = configuration.CustomValues.FirstOrDefault(v => string.Equals(v.Key, key))?.Value;
            if (string.IsNullOrEmpty(value))
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{actionType}\" spell action.");

            return value;
        }

        public abstract JsonData GetJson();
    }
}