using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Spells.SpellActions;
using Jint;
using Jint.Native;
using Jint.Parser;
using Jint.Runtime;

namespace CodeMagic.Core.Spells.Script
{
    public class SpellCodeExecutor
    {
        private const int ScanForWallsManaCostMultiplier = 1;
        private const int ScanForObjectsManaCostMultiplier = 3;

        private readonly string code;
        private readonly Engine jsEngine;
        private int lifeTime;

        private readonly Dictionary<string, object> globalData;

        private readonly ICreatureObject caster;

        public SpellCodeExecutor(ICreatureObject caster, string code)
        {
            this.code = code;
            this.caster = caster;

            jsEngine = new Engine();
            ConfigureEngine();

            lifeTime = 0;

            globalData = new Dictionary<string, object>();
        }

        private void ConfigureEngine()
        {
            jsEngine.SetValue("getCaster", new Func<JsValue>(() => ConvertDestroyable(caster).ToJson(jsEngine)));

            jsEngine.SetValue("storeValue", new Action<string, object>((key, data) => globalData.Add(key, data)));
            jsEngine.SetValue("getStoredValue", new Func<string, object>(key => globalData.ContainsKey(key) ? globalData[key] : null));

            // Spell actions
            jsEngine.SetValue("move", new Func<string, int, JsValue>(GetMoveSpellAction));
            jsEngine.SetValue("buildWall", new Func<int, JsValue>(GetBuildWallSpellAction));
            jsEngine.SetValue("heat", new Func<int, JsValue>(GetHeatAreaSpellAction));
            jsEngine.SetValue("cool", new Func<int, JsValue>(GetCoolAreaSpellAction));
        }

        public ISpellAction Execute(IAreaMap map, Point position, CodeSpell spell)
        {
            var result = ExecuteCode(map, position, spell);
            if (result == null)
            {
                throw new SpellException("Main spell function did not return spell action.");
            }

            var action = new SpellActionsFactory().GetSpellAction(result, spell);
            if (action == null)
            {
                throw new SpellException("Spell returned no action.");
            }

            lifeTime++;

            return action;
        }

        private dynamic ExecuteCode(IAreaMap map, Point position, CodeSpell spell)
        {
            ConfigureDynamicEngineFunctions(map, position, spell);

            JsValue mainFunction = null;
            try
            {
                mainFunction = jsEngine.Execute(code).GetValue("main");
            }
            catch (ParserException ex)
            {
                throw new SpellException($"Error in spell code {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new SpellException($"Unknown error during spell execution: {ex.Message}", ex);
            }
            
            if (mainFunction == null)
            {
                throw new SpellException("Function \"main\" not found in spell code.");
            }

            dynamic result;
            try
            {
                result = mainFunction.Invoke(lifeTime).ToObject() as ExpandoObject;
            }
            catch (JavaScriptException ex)
            {
                throw new SpellException($"Error in spell code: [line {ex.LineNumber}] {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new SpellException($"Unknown error during spell execution: {ex.Message}", ex);
            }

            return result;
        }

        private void ConfigureDynamicEngineFunctions(IAreaMap map, Point position, CodeSpell spell)
        {
            jsEngine.SetValue("getMana", new Func<int>(() => spell.Mana));
            jsEngine.SetValue("getPosition", new Func<JsValue>(() => ConvertPoint(position)));
            jsEngine.SetValue("getTemperature", new Func<int>(() => map.GetCell(position).Temperature.Value));
            jsEngine.SetValue("getIsSolidWall",
                new Func<string, bool>((direction) => GetIfCellIsSolid(map, position, direction)));
            jsEngine.SetValue("getAreObjectsUnder", new Func<bool>(() => GetAreObjectsUnder(map, position)));

            jsEngine.SetValue("scanForWalls",
                new Func<int, bool[][]>(radius => ScanForWalls(map, position, radius, spell)));
            jsEngine.SetValue("scanForObjects",
                new Func<int, JsValue[][][]>(radius => ScanForObjects(map, position, radius, spell)));

        }

        private JsValue[][][] ScanForObjects(IAreaMap map, Point position, int radius, CodeSpell spell)
        {
            var cost = radius * ScanForObjectsManaCostMultiplier;
            if (spell.Mana < cost)
            {
                spell.Mana = 0;
                return null;
            }

            spell.Mana -= cost;
            var mapSegment = map.GetMapPart(position, radius);
            return mapSegment.Select(row =>
                row.Select(cell =>
                    cell.Objects.OfType<IDestroyableObject>().Select(obj => ConvertDestroyable(obj).ToJson(jsEngine))
                        .ToArray()).ToArray()).ToArray();
        }

        private bool[][] ScanForWalls(IAreaMap map, Point position, int radius, CodeSpell spell)
        {
            var cost = radius * ScanForWallsManaCostMultiplier;
            if (spell.Mana < cost)
            {
                spell.Mana = 0;
                return null;
            }

            spell.Mana -= cost;
            var mapSegment = map.GetMapPart(position, radius);
            return mapSegment.Select(row => row.Select(cell => cell == null || cell.BlocksProjectiles).ToArray())
                .ToArray();
        }

        private bool GetAreObjectsUnder(IAreaMap map, Point position)
        {
            var cell = map.GetCell(position);
            return cell.Objects.OfType<IDestroyableObject>().Any();
        }

        private JsValue ConvertPoint(Point point)
        {
            return new JsonData(new Dictionary<string, object>
            {
                {"x", point.X},
                {"y", point.Y}
            }).ToJson(jsEngine);
        }

        #region Code Spell API

        private JsonData ConvertDestroyable(IDestroyableObject destroyable)
        {
            var data = new JsonData(new Dictionary<string, object>
            {
                {"id", destroyable.Id},
                {"health", destroyable.Health},
                {"maxHealth", destroyable.MaxHealth}
            });

            if (destroyable is ICreatureObject creature)
            {
                data.Data.Add("direction", creature.Direction);
            }

            return data;
        }

        private bool GetIfCellIsSolid(IAreaMap map, Point position, string directionString)
        {
            var parsedDirection = SpellHellper.ParseDirection(directionString);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown direction value: {directionString}");

            var checkPosition = Point.GetAdjustedPoint(position, parsedDirection.Value);
            if (!map.ContainsCell(checkPosition))
                return true;

            return map.GetCell(checkPosition).BlocksProjectiles;
        }

        #endregion

        #region Action Creation functions

        private JsValue GetMoveSpellAction(string direction, int distance)
        {
            return MoveSpellAction.GetJson(direction, distance).ToJson(jsEngine);
        }

        private JsValue GetBuildWallSpellAction(int time)
        {
            return BuildWallSpellAction.GetJson(time).ToJson(jsEngine);
        }

        private JsValue GetHeatAreaSpellAction(int value)
        {
            return HeatAreaSpellAction.GetJson(value).ToJson(jsEngine);
        }

        private JsValue GetCoolAreaSpellAction(int value)
        {
            return CoolAreaSpellAction.GetJson(value).ToJson(jsEngine);
        }

        #endregion
    }
}