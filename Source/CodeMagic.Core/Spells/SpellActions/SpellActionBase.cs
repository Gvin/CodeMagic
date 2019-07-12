using System;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Spells.SpellActions
{
    public abstract class SpellActionBase : ISpellAction
    {
        private readonly double manaCostMultiplier;
        private readonly int manaCostPower;

        protected SpellActionBase(string actionType)
        {
            var configuration = Configuration.ConfigurationManager.GetSpellConfiguration(actionType);
            if (configuration == null)
                throw new ApplicationException($"Configuration for spell action \"{actionType}\" not found.");

            manaCostMultiplier = configuration.ManaCostMultiplier;
            manaCostPower = configuration.ManaCostPower;
        }

        public abstract Point Perform(IGameCore game, Point position);

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
            var configuration = Configuration.ConfigurationManager.GetSpellConfiguration(actionType);
            if (configuration == null)
                throw new ApplicationException($"Configuration for spell action \"{actionType}\" not found.");

            return GetManaCost(value, configuration.ManaCostMultiplier, configuration.ManaCostPower);
        }
    }
}