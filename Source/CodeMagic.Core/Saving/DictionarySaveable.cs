using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Saving
{
    public class DictionarySaveable : ISaveable
    {
        private const string SaveKeyData = "Data";

        public DictionarySaveable(SaveData data)
        {
            Data = data.GetObjectsCollection<DictionaryPairSaveable>(SaveKeyData)
                .Select(pair => new KeyValuePair<object, object>(pair.Key, pair.Value)).ToArray();
        }

        public DictionarySaveable(Dictionary<object, object> data)
        {
            Data = data.ToArray();
        }

        public KeyValuePair<object, object>[] Data { get; }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyData, Data.Select(pair => new DictionaryPairSaveable(pair)).ToArray()}
            });
        }

        public class DictionaryPairSaveable : ISaveable
        {
            private const string SaveKeyKey = "Key";
            private const string SaveKeyValue = "Value";

            public DictionaryPairSaveable(SaveData data)
            {
                Key = data.Values[SaveKeyKey];
                Value = data.Values[SaveKeyValue];
            }

            public DictionaryPairSaveable(KeyValuePair<object, object> data)
            {
                Key = data.Key;
                Value = data.Value;
            }

            public object Key { get; }

            public object Value { get; }

            public SaveDataBuilder GetSaveData()
            {
                return new SaveDataBuilder(GetType(), new Dictionary<string, object>
                {
                    {SaveKeyKey, Key},
                    {SaveKeyValue, Value}
                });
            }
        }
    }
}