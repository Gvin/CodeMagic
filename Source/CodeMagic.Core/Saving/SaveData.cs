using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Saving
{
    public class SaveData
    {
        private static IDataSerializer serializer;

        public static void Init(IDataSerializer dataSerializer)
        {
            serializer = dataSerializer;
        }

        public SaveData()
        {
            Values = new Dictionary<string, string>();
            Objects = new Dictionary<string, SaveData>();
            ObjectsCollections = new Dictionary<string, SaveData[]>();
            ValuesCollections = new Dictionary<string, string[]>();
        }

        public string Type { get; set; }

        public Dictionary<string, string> Values { get; set; }

        public Dictionary<string, SaveData> Objects { get; set; }

        public Dictionary<string, SaveData[]> ObjectsCollections { get; set; }

        public Dictionary<string, string[]> ValuesCollections { get; set; }

        public object[] GetObjectsCollection(string key)
        {
            var dataObjects = ObjectsCollections[key].Cast<SaveData>();
            return dataObjects.Select(DeserializeObject).ToArray();
        }

        public string[] GetValuesCollection(string key)
        {
            return ValuesCollections[key];
        }

        public T[] GetObjectsCollection<T>(string key) where T : class, ISaveable
        {
            var dataObjects = ObjectsCollections[key].Cast<SaveData>();
            return dataObjects.Select(data => (T)DeserializeObject(data)).ToArray();
        }

        public object GetObject(string key)
        {
            if (!Objects.ContainsKey(key))
                return null;
            var data = Objects[key];
            if (data == null)
                return null;
            return DeserializeObject((SaveData)data);
        }

        public T GetSerializedObject<T>(string key) where T : class
        {
            var rawData = Values[key];
            return serializer.Deserialize<T>(rawData);
        }

        private object DeserializeObject(SaveData dataBuilder)
        {
            var type = System.Type.GetType(dataBuilder.Type);
            if (type == null)
                throw new ApplicationException($"Type {dataBuilder.Type} not found in the solution.");

            return Activator.CreateInstance(type, dataBuilder);
        }

        public T GetObject<T>(string key) where T : class, ISaveable
        {
            var obj = GetObject(key);
            return (T)obj;
        }

        public string GetStringValue(string key)
        {
            return Values[key];
        }

        public bool GetBoolValue(string key)
        {
            return string.Equals(Values[key], bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }

        public int GetIntValue(string key)
        {
            return int.Parse(Values[key]);
        }
    }
}