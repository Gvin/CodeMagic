using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class BuildWallSpellAction : ISpellAction
    {
        public const string ActionType = "build_wall";
        private const int ManaCostMultiplier = 2;

        private readonly int time;

        public BuildWallSpellAction(dynamic actionData)
        {
            time = (int) actionData.time;
        }

        public Point Perform(IAreaMap map, Point position, Journal journal)
        {
            var cell = map.GetCell(position);
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

        private static int GetManaCost(int time)
        {
            return time * ManaCostMultiplier;
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