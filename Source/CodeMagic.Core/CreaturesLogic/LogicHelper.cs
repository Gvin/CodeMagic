using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic
{
    public static class LogicHelper
    {
        public static bool GetIfPlayerVisible(INonPlayableCreatureObject creature, Point position)
        {
            var playerPosition = CurrentGame.Map.GetObjectPosition<IPlayer>();
            if (playerPosition == null)
                return false;
            return
                CreaturesVisibilityHelper.GetIfPointIsVisible(position, creature.VisibilityRange, playerPosition);
        }

        public static int GetHealthPercent(INonPlayableCreatureObject creature)
        {
            return (int) Math.Ceiling(creature.Health / (double) creature.MaxHealth);
        }
    }
}