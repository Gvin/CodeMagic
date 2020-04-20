using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Saving
{
    public class SaveDataBuilder
    {
        private readonly Dictionary<string, object> rawData;
        private readonly string type;

        public SaveDataBuilder(Type type)
            : this(type, new Dictionary<string, object>())
        {
        }

        public SaveDataBuilder(Type type, Dictionary<string, object> data)
        {
            rawData = data;
            this.type = type.AssemblyQualifiedName;
        }

        public SaveData ConvertRawData(IDataSerializer serializer)
        {
            var result = new SaveData
            {
                Type = type
            };

            foreach (var pair in rawData)
            {
                switch (pair.Value)
                {
                    case ISaveable saveable:
                        var saveDataBuilder = saveable.GetSaveData();
                        result.Objects.Add(pair.Key, saveDataBuilder.ConvertRawData(serializer));
                        break;
                    case string str:
                        result.Values.Add(pair.Key, str);
                        break;
                    case IEnumerable collection:
                        var array = collection.Cast<object>().ToArray();
                        if (array.All(elem => elem is ISaveable))
                        {
                            result.ObjectsCollections.Add(pair.Key, array.Cast<ISaveable>().Select(elem => elem.GetSaveData().ConvertRawData(serializer)).ToArray());
                        }
                        else
                        {
                            result.ValuesCollections.Add(pair.Key, array.Select(elem => GetStringValue(elem, serializer)).ToArray());
                        }
                        break;
                    default:
                        result.Values.Add(pair.Key, GetStringValue(pair.Value, serializer));
                        break;
                }
            }

            return result;
        }

        private string GetStringValue(object value, IDataSerializer serializer)
        {
            if (value == null)
                return null;

            if (value is string str)
                return str;

            return serializer.Serialize(value);
        }
    }

    public interface IDataSerializer
    {
        string Serialize(object data);

        T Deserialize<T>(string dataString) where T : class;
    }
}