﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects;
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

        private readonly string casterId;

        public SpellCodeExecutor(string casterId, string code)
        {
            this.code = code;
            this.casterId = casterId;

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

        public ISpellAction Execute(Point position, CodeSpell spell, int lifeTime)
        {
            var result = ExecuteCode(position, spell, lifeTime);
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

        private dynamic ExecuteCode(Point position, CodeSpell spell, int lifeTime)
        {
            ConfigureDynamicEngineFunctions(position, spell);

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

        private void ConfigureDynamicEngineFunctions(Point position, CodeSpell spell)
        {
            jsEngine.SetValue("getLightLevel", new Func<int>(() => GetLightLevel(position)));
            jsEngine.SetValue("getCaster", new Func<JsValue>(() => ConvertDestroyable(CurrentGame.Map.GetDestroyableObject(casterId)).ToJson(jsEngine)));
            jsEngine.SetValue("log", new Action<object>(message => LogMessage(spell, message)));
            jsEngine.SetValue("getMana", new Func<int>(() => spell.Mana));
            jsEngine.SetValue("getPosition", new Func<JsValue>(() => ConvertPoint(position)));
            jsEngine.SetValue("getTemperature", new Func<int>(() => CurrentGame.Map.GetCell(position).Temperature()));
            jsEngine.SetValue("getIsSolidWall",
                new Func<string, bool>((direction) => GetIfCellIsSolid(position, direction)));
            jsEngine.SetValue("getObjectsUnder", new Func<JsValue[]>(() => GetObjectsUnder(position)));

            jsEngine.SetValue("scanForWalls",
                new Func<int, bool[][]>(radius => ScanForWalls(position, radius, spell)));
            jsEngine.SetValue("scanForObjects",
                new Func<int, JsValue[]>(radius => ScanForObjects(position, radius, spell)));
        }

        private void LogMessage(CodeSpell spell, object message)
        {
            CurrentGame.Journal.Write(new SpellLogMessage(spell.Name, GetMessageString(message)));
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

        private int GetLightLevel(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);
            return (int)cell.LightLevel;
        }

        private JsValue[] ScanForObjects(Point position, int radius, CodeSpell spell)
        {
            var cost = radius * ScanForObjectsManaCostMultiplier;
            if (spell.Mana < cost)
            {
                spell.Mana = 0;
                return null;
            }

            spell.Mana -= cost;
            var mapSegment = CurrentGame.Map.GetMapPart(position, radius);
            return mapSegment.SelectMany(row => 
                    row.Where(cell => cell != null)
                        .SelectMany(cell =>
                        cell.Objects
                            .OfType<IDestroyableObject>()
                            .Select(obj => ConvertDestroyable(obj).ToJson(jsEngine))))
                .ToArray();
        }

        private bool[][] ScanForWalls(Point position, int radius, CodeSpell spell)
        {
            var cost = radius * ScanForWallsManaCostMultiplier;
            if (spell.Mana < cost)
            {
                spell.Mana = 0;
                return null;
            }

            spell.Mana -= cost;
            var mapSegment = CurrentGame.Map.GetMapPart(position, radius);
            return mapSegment.Select(row => row.Select(cell => cell == null || cell.BlocksProjectiles).ToArray())
                .ToArray();
        }

        private JsValue[] GetObjectsUnder(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);
            return cell.Objects.OfType<IDestroyableObject>().Select(obj => ConvertDestroyable(obj).ToJson(jsEngine))
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

        private JsonData ConvertDestroyable(IDestroyableObject destroyable)
        {
            var position = CurrentGame.Map.GetObjectPosition(obj => obj.Equals(destroyable));
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
            if (destroyable.Equals(CurrentGame.Map.GetDestroyableObject(casterId)))
            {
                return "caster";
            }

            if (destroyable is ICreatureObject)
            {
                return "creature";
            }

            return "object";
        }

        private bool GetIfCellIsSolid(Point position, string directionString)
        {
            var parsedDirection = SpellHelper.ParseDirection(directionString);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown direction value: {directionString}");

            var checkPosition = Point.GetPointInDirection(position, parsedDirection.Value);
            if (!CurrentGame.Map.ContainsCell(checkPosition))
                return true;

            return CurrentGame.Map.GetCell(checkPosition).BlocksProjectiles;
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