using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Buildings;
using CodeMagic.Game.Spells.SpellActions;
using Jint;
using Jint.Native;
using Jint.Parser;
using Jint.Runtime;

namespace CodeMagic.Game.Spells.Script
{
    public class SpellCodeExecutor
    {
        private const int ScanForWallsManaCostMultiplier = 1;
        private const int ScanForObjectsManaCostMultiplier = 3;

        private readonly string code;
        private readonly Engine jsEngine;

        private readonly Dictionary<string, object> globalData;

        private readonly ICreatureObject caster;

        public SpellCodeExecutor(ICreatureObject caster, string code)
        {
            this.code = code;
            this.caster = caster;

            jsEngine = new Engine();
            ConfigureEngine();

            globalData = new Dictionary<string, object>();
        }

        private void ConfigureEngine()
        {
            
            jsEngine.SetValue("storeValue", new Action<string, object>(StoreData));
            jsEngine.SetValue("getStoredValue", new Func<string, object>(key => globalData.ContainsKey(key) ? globalData[key] : null));

            // Spell actions
            jsEngine.SetValue("move", new Func<string, int, JsValue>(GetMoveSpellAction));
            jsEngine.SetValue("buildWall", new Func<int, JsValue>(GetBuildWallSpellAction));
            jsEngine.SetValue("heat", new Func<int, JsValue>(GetHeatAreaSpellAction));
            jsEngine.SetValue("cool", new Func<int, JsValue>(GetCoolAreaSpellAction));
            jsEngine.SetValue("push", new Func<string, int, JsValue>(GetPushSpellAction));
            jsEngine.SetValue("compress", new Func<int, JsValue>(GetCompressSpellAction));
            jsEngine.SetValue("decompress", new Func<int, JsValue>(GetDecompressSpellAction));
            jsEngine.SetValue("createWater", new Func<int, JsValue>(GetCreateWaterSpellAction));
            jsEngine.SetValue("longCast", new Func<dynamic, string, int, JsValue>(GetLongCastSpellAction));
            jsEngine.SetValue("transformWater", new Func<string, int, JsValue>(GetTransformWaterSpellAction));
            jsEngine.SetValue("shock", new Func<int, JsValue>(GetShockSpellAction));
            jsEngine.SetValue("emitLight", new Func<int, int, JsValue>(GetEmitLightSpellAction));
        }

        public ISpellAction Execute(IAreaMap map, IJournal journal, Point position, CodeSpell spell, int lifeTime)
        {
            var result = ExecuteCode(map, journal, position, spell, lifeTime);
            if (result == null)
            {
                return new EmptySpellAction();
            }

            var action = new SpellActionsFactory().GetSpellAction(result, spell);
            if (action == null)
            {
                throw new SpellException("Spell returned no action.");
            }

            return action;
        }

        private dynamic ExecuteCode(IAreaMap map, IJournal journal, Point position, CodeSpell spell, int lifeTime)
        {
            ConfigureDynamicEngineFunctions(map, journal, position, spell);

            JsValue mainFunction;
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

        private void ConfigureDynamicEngineFunctions(IAreaMap map, IJournal journal, Point position, CodeSpell spell)
        {
            jsEngine.SetValue("getLightLevel", new Func<int>(() => GetLightLevel(map, position)));
            jsEngine.SetValue("getCaster", new Func<JsValue>(() => ConvertDestroyable(caster, map).ToJson(jsEngine)));
            jsEngine.SetValue("log", new Action<object>(message => LogMessage(journal, spell, message)));
            jsEngine.SetValue("getMana", new Func<int>(() => spell.Mana));
            jsEngine.SetValue("getPosition", new Func<JsValue>(() => ConvertPoint(position)));
            jsEngine.SetValue("getTemperature", new Func<int>(() => map.GetCell(position).Temperature()));
            jsEngine.SetValue("getHumidity", new Func<double>(() => GetHumidity(map, position)));
            jsEngine.SetValue("getIsSolidWall",
                new Func<string, bool>((direction) => GetIfCellIsSolid(map, position, direction)));
            jsEngine.SetValue("getObjectsUnder", new Func<JsValue[]>(() => GetObjectsUnder(map, position)));

            jsEngine.SetValue("scanForWalls",
                new Func<int, bool[][]>(radius => ScanForWalls(map, position, radius, spell)));
            jsEngine.SetValue("scanForObjects",
                new Func<int, JsValue[]>(radius => ScanForObjects(map, position, radius, spell)));
        }

        private void LogMessage(IJournal journal, CodeSpell spell, object message)
        {
            journal.Write(new SpellLogMessage(spell.Name, GetMessageString(message)));
        }

        private void StoreData(string key, object data)
        {
            if (globalData.ContainsKey(key))
            {
                globalData[key] = data;
                return;
            }
            globalData.Add(key, data);
        }

        private string GetMessageString(object message)
        {
            switch (message)
            {
                case null:
                    return "null";
                case string text:
                    return text;
                case int number:
                    return number.ToString();
                case object[] array:
                    var processedArray = string.Join(", ", array.Select(GetMessageString));
                    return $"[{processedArray}]";
                case ExpandoObject _:
                    return "object";
                default:
                    return message.ToString();
            }
        }

        private int GetLightLevel(IAreaMap map, Point position)
        {
            var cell = map.GetCell(position);
            return (int)cell.LightLevel;
        }

        private JsValue[] ScanForObjects(IAreaMap map, Point position, int radius, CodeSpell spell)
        {
            var cost = radius * ScanForObjectsManaCostMultiplier;
            if (spell.Mana < cost)
            {
                spell.Mana = 0;
                return null;
            }

            spell.Mana -= cost;
            var mapSegment = map.GetMapPart(position, radius);
            return mapSegment.SelectMany(row => 
                    row.Where(cell => cell != null)
                        .SelectMany(cell =>
                        cell.Objects
                            .OfType<IDestroyableObject>()
                            .Select(obj => ConvertDestroyable(obj, map).ToJson(jsEngine))))
                .ToArray();
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

        private JsValue[] GetObjectsUnder(IAreaMap map, Point position)
        {
            var cell = map.GetCell(position);
            return cell.Objects.OfType<IDestroyableObject>().Select(obj => ConvertDestroyable(obj, map).ToJson(jsEngine))
                .ToArray();
        }

        #region Code Spell API

        private JsValue ConvertPoint(Point point)
        {
            return new JsonData(new Dictionary<string, object>
            {
                {"x", point.X},
                {"y", point.Y}
            }).ToJson(jsEngine);
        }

        private JsonData ConvertDestroyable(IDestroyableObject destroyable, IAreaMap map)
        {
            var position = map.GetObjectPosition(obj => obj.Equals(destroyable));
            var data = new JsonData(new Dictionary<string, object>
            {
                {"id", destroyable.Id},
                {"health", destroyable.Health},
                {"maxHealth", destroyable.MaxHealth},
                {"position", position},
                {"type", GetObjectType(destroyable)}
            });

            if (destroyable is ICreatureObject creature)
            {
                data.Data.Add("direction", creature.Direction);
            }

            return data;
        }

        private string GetObjectType(IDestroyableObject destroyable)
        {
            if (destroyable.Equals(caster))
            {
                return "caster";
            }

            if (destroyable is ICreatureObject)
            {
                return "creature";
            }

            return "object";
        }

        private bool GetIfCellIsSolid(IAreaMap map, Point position, string directionString)
        {
            var parsedDirection = SpellHelper.ParseDirection(directionString);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown direction value: {directionString}");

            var checkPosition = Point.GetPointInDirection(position, parsedDirection.Value);
            if (!map.ContainsCell(checkPosition))
                return true;

            return map.GetCell(checkPosition).BlocksProjectiles;
        }

        private double GetHumidity(IAreaMap map, Point position)
        {
            var cell = map.GetCell(position);
            var growingPlace = cell.Objects.OfType<GrowingPlace>().FirstOrDefault();
            return growingPlace?.Humidity ?? 0d;
        }

        #endregion

        #region Action Creation functions

        private JsValue GetEmitLightSpellAction(int power, int time)
        {
            return EmitLightSpellAction.GetJson(power, time).ToJson(jsEngine);
        }

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

        private JsValue GetPushSpellAction(string direction, int force)
        {
            return PushSpellAction.GetJson(direction, force).ToJson(jsEngine);
        }

        private JsValue GetCompressSpellAction(int pressure)
        {
            return CompressSpellAction.GetJson(pressure).ToJson(jsEngine);
        }

        private JsValue GetDecompressSpellAction(int pressure)
        {
            return DecompressSpellAction.GetJson(pressure).ToJson(jsEngine);
        }

        private JsValue GetCreateWaterSpellAction(int volume)
        {
            return CreateWaterSpellAction.GetJson(volume).ToJson(jsEngine);
        }

        private JsValue GetLongCastSpellAction(dynamic actionData, string direction, int distance)
        {
            return LongCastSpellAction.GetJson(actionData, direction, distance).ToJson(jsEngine);
        }

        private JsValue GetTransformWaterSpellAction(string resultLiquid, int volume)
        {
            return TransformWaterSpellAction.GetJson(resultLiquid, volume).ToJson(jsEngine);
        }

        private JsValue GetShockSpellAction(int power)
        {
            return ShockSpellAction.GetJson(power).ToJson(jsEngine);
        }

        #endregion
    }
}