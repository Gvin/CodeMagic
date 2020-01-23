using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace CodeMagic.Game.Saving.Common
{
    [Serializable]
    public class XmlDictionary<TKey, TValue>
    {
        public XmlDictionary()
        {
        }

        public XmlDictionary(Dictionary<TKey, TValue> data)
        {
            Pairs = data.Select(pair => new XmlKeyValuePair<TKey, TValue>
                {
                    Key = pair.Key,
                    Value = pair.Value
                })
                .ToArray();
        }

        [XmlElement("pair")]
        public XmlKeyValuePair<TKey, TValue>[] Pairs { get; set; }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return Pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    [Serializable]
    public class XmlKeyValuePair<TKey, TValue>
    {
        [XmlElement("key")]
        public TKey Key { get; set; }

        [XmlElement("value")]
        public TValue Value { get; set; }
    }
}