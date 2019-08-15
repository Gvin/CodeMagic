using System;
using System.Collections.Generic;
using System.Text;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using Jint;
using Jint.Native;
using Jint.Native.Json;

namespace CodeMagic.Core.Spells.Script
{
    public class JsonData
    {
        public JsonData()
        {
            Data = new Dictionary<string, object>();
        }

        public JsonData(Dictionary<string, object> data)
        {
            Data = data;
        }

        public Dictionary<string, object> Data;

        public JsValue ToJson(Engine jsEngine)
        {
            var jsonString = GetJsonString(jsEngine);
            return new JsonParser(jsEngine).Parse(jsonString);
        }

        private string GetJsonString(Engine jsEngine)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");

            foreach (var pair in Data)
            {
                builder.AppendLine($"\"{pair.Key}\": {GetJsonValue(pair.Value, jsEngine)},");
            }

            builder.AppendLine("}");
            return builder.ToString();
        }

        private string GetJsonValue(object value, Engine jsEngine)
        {
            if (value is int number)
            {
                return number.ToString();
            }

            if (value is string text)
            {
                return $"\"{text}\"";
            }

            if (value is Point point)
            {
                return GetPointJson(point);
            }

            if (value is Direction direction)
            {
                return $"\"{GetDirectionString(direction)}\"";
            }

            if (value is JsonData json)
            {
                return json.GetJsonString(jsEngine);
            }

            return $"\"{value}\"";
        }

        private string GetPointJson(Point point)
        {
            return $"{{ \"x\": {point.X}, \"y\": {point.Y} }}";
        }

        public static string GetDirectionString(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return "north";
                case Direction.South:
                    return "south";
                case Direction.West:
                    return "west";
                case Direction.East:
                    return "east";
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unknown direction value.");
            }
        }
    }
}