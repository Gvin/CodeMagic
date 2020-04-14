﻿using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public class CompressSpellAction : SpellActionBase
    {
        public const string ActionType = "compress";

        private readonly int pressure;

        public CompressSpellAction(dynamic actionData)
            : base(ActionType)
        {
            pressure = (int) actionData.pressure;
        }

        public override Point Perform(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);
            if (cell.BlocksEnvironment)
                return position;

            cell.Environment.Cast().Pressure += pressure;
            return position;
        }

        public override int ManaCost => GetManaCost(pressure);

        public override JsonData GetJson()
        {
            return GetJson(pressure);
        }

        public static JsonData GetJson(int pressure)
        {
            if (pressure <= 0)
                throw new SpellException("Pressure value should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"pressure", pressure},
                {"manaCost", GetManaCost(ActionType, pressure)}
            });
        }
    }
}