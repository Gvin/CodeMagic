using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Injection;
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

        public override Point Perform(IAreaMap map, IJournal journal, Point position)
        {
            var wall = Injector.Current.Create<IEnergyWall>(time);
            map.AddObject(position, wall);
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