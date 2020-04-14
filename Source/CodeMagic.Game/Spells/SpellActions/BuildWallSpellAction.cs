using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public class BuildWallSpellAction : SpellActionBase
    {
        public const string ActionType = "build_wall";

        private readonly int time;

        public BuildWallSpellAction(dynamic actionData)
            : base(ActionType)
        {
            time = (int) actionData.time;
        }

        public override Point Perform(Point position)
        {
            var wall = new EnergyWall(time);
            CurrentGame.Map.AddObject(position, wall);
            return position;
        }

        public override int ManaCost => GetManaCost(time);

        public override JsonData GetJson()
        {
            return GetJson(time);
        }

        public static JsonData GetJson(int time)
        {
            if (time <= 0)
                throw new SpellException("Wall existence time should be greater than 0.");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"time", time},
                {"manaCost", GetManaCost(ActionType, time)}
            });
        }
    }
}