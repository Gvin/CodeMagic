using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game
{
    public static class AttackHelper
    {
        public static int CalculateDamage(int damage, Element element, Player player)
        {
            if (element == Element.Piercing ||
                element == Element.Slashing ||
                element == Element.Blunt)
            {
                return (int) Math.Round(damage * (1d + player.DamageBonus / 100d));
            }

            return damage;
        }
    }
}