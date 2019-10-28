using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic
{
    public static class LogicHelper
    {
        public static bool GetIfPlayerVisible(INonPlayableCreatureObject creature, IAreaMap map, Point position)
        {
            var playerPosition = map.GetObjectPosition<IPlayer>();
            if (playerPosition == null)
                return false;
            return
                CreaturesVisibilityHelper.GetIfPointIsVisible(map, position, creature.VisibilityRange, playerPosition);
        }

        public static int GetHealthPercent(INonPlayableCreatureObject creature)
        {
            return (int) Math.Ceiling(creature.Health / (double) creature.MaxHealth);
        }
    }
}