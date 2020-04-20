using System.Collections.Generic;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Objects
{
    public abstract class MapObjectBase : IMapObject
    {
        private const string SaveKeyName = "Name";

        protected MapObjectBase(SaveData data)
        {
            Name = data.GetStringValue(SaveKeyName);
        }

        protected MapObjectBase(string name)
        {
            Name = name;
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), GetSaveDataContent());
        }

        protected virtual Dictionary<string, object> GetSaveDataContent()
        {
            return new Dictionary<string, object>
            {
                {SaveKeyName, Name}
            };
        }

        public string Name { get; }

        public virtual bool BlocksMovement => false;

        public virtual bool BlocksProjectiles => false;

        public virtual bool IsVisible => true;

        public virtual bool BlocksVisibility => false;

        public virtual bool BlocksAttack => false;

        public virtual bool BlocksEnvironment => false;

        public abstract ZIndex ZIndex { get; }

        public abstract ObjectSize Size { get; }

        public virtual bool Equals(IMapObject other)
        {
            return ReferenceEquals(other, this);
        }
    }
}