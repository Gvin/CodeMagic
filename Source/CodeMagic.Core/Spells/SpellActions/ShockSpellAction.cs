using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Spells.Script;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class ShockSpellAction : SpellActionBase
    {
        private const string CustomValueWaterTransferMultiplier = "WaterTransferMultiplier";
        private const string CustomValueHeatMultiplier = "HeatMultiplier";
        public const string ActionType = "shock";

        private readonly int power;
        private readonly double waterTransferMultiplier;
        private readonly int heatMultiplier;

        public ShockSpellAction(dynamic actionData) 
            : base(ActionType)
        {
            power = (int) actionData.power;
            waterTransferMultiplier = GetWaterTransferMultiplier();
            heatMultiplier = GetHeatMultiplier();
        }

        public override Point Perform(IGameCore game, Point position)
        {
            var currentCell = game.Map.GetCell(position);
            ApplyShockToCell(power, currentCell, game.Journal);

            if (!GetIfCellConductsElectricity(currentCell)) // No water - no spreading
                return position;

            var processedCells = new List<Point> {position};
            var spreadPower = (int) Math.Floor(power * waterTransferMultiplier);
            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Up), spreadPower, processedCells);
            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Down), spreadPower, processedCells);
            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Left), spreadPower, processedCells);
            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Right), spreadPower, processedCells);

            return position;
        }

        private void ProcessShockSpreadToCell(IGameCore game, Point position, int value, List<Point> processedCells)
        {
            if (value == 0)
                return;

            if (processedCells.Any(pos => pos.Equals(position)))
                return;

            processedCells.Add(position);
            if (!game.Map.ContainsCell(position))
                return;

            var currentCell = game.Map.GetCell(position);
            if (!GetIfCellConductsElectricity(currentCell)) // No water - no spreading
                return;

            processedCells.Add(position);
            ApplyShockToCell(value, currentCell, game.Journal);

            var spreadPower = (int)Math.Floor(value * waterTransferMultiplier);

            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Up), spreadPower, processedCells);
            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Down), spreadPower, processedCells);
            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Left), spreadPower, processedCells);
            ProcessShockSpreadToCell(game, Point.GetPointInDirection(position, Direction.Right), spreadPower, processedCells);
        }

        private bool GetIfCellConductsElectricity(AreaMapCell cell)
        {
            return GetIfCellContainsWater(cell) || GetIfCellContainsWet(cell);
        }

        private bool GetIfCellContainsWater(AreaMapCell cell)
        {
            return cell.Objects.OfType<WaterLiquidObject>().Any();
        }

        private bool GetIfCellContainsWet(AreaMapCell cell)
        {
            return cell.Objects.OfType<IDestroyableObject>()
                .Any(obj => obj.Statuses.Contains(WetObjectStatus.StatusType));
        }

        private void ApplyShockToCell(int value, AreaMapCell cell, Journal journal)
        {
            var heat = value * heatMultiplier;
            cell.Environment.Temperature += heat;

            var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var destroyable in destroyableObjects)
            {
                destroyable.Damage(value, Element.Electricity);
                journal.Write(new EnvironmentDamageMessage(destroyable, value, Element.Electricity));
            }
        }

        private double GetWaterTransferMultiplier()
        {
            var stringValue = GetCustomValue(CustomValueWaterTransferMultiplier);
            return double.Parse(stringValue);
        }

        private int GetHeatMultiplier()
        {
            var stringValue = GetCustomValue(CustomValueHeatMultiplier);
            return int.Parse(stringValue);
        }

        public override int ManaCost => GetManaCost(power);

        public override JsonData GetJson()
        {
            return GetJson(power);
        }

        public static JsonData GetJson(int power)
        {
            if (power <= 0)
                throw new SpellException("Shock power should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"power", power},
                {"manaCost", GetManaCost(ActionType, power)}
            });
        }
    }
}