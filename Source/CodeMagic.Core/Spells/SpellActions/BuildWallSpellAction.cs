using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class BuildWallSpellAction : ISpellAction
    {
        public const string ActionType = "build_wall";
        private const int ManaCostPower = 2;

        private readonly int time;

        public BuildWallSpellAction(dynamic actionData)
        {
            time = (int) actionData.time;
        }

        public Point Perform(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            if (cell.BlocksMovement)
                return position;

            var wall = new EnergyWall(new EnergyWallConfiguration
            {
                Name = "Energy Wall",
                LifeTime = time
            });
            cell.Objects.Add(wall);
            return position;
        }

        public int ManaCost => GetManaCost(time);

        /// <remarks>
        /// 1 - 1
        /// 2 - 4
        /// 3 - 8
        /// </remarks>
        private static int GetManaCost(int time)
        {
            return (int)Math.Pow(time, ManaCostPower);
        }

        public static JsonData GetJson(int time)
        {
            if (time <= 0)
                throw new SpellException("Wall existence time should be greater than 0.");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"time", time},
                {"manaCost", GetManaCost(time)}
            });
        }
    }
}