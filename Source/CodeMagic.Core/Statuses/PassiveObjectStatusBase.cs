using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Statuses
{
    public abstract class PassiveObjectStatusBase : IObjectStatus
    {
        private const string SaveDataLifeTime = "LifeTime";

        private readonly int maxLifeTime;
        private int lifeTime;

        protected PassiveObjectStatusBase(SaveData data, int maxLifeTime)
        {
            this.maxLifeTime = maxLifeTime;
            lifeTime = data.GetIntValue(SaveDataLifeTime);
        }

        protected PassiveObjectStatusBase(int maxLifeTime)
        {
            this.maxLifeTime = maxLifeTime;
            lifeTime = 0;
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveDataLifeTime, lifeTime}
            });
        }

        public bool Update(IDestroyableObject owner, Point position)
        {
            if (lifeTime >= maxLifeTime)
            {
                return false;
            }

            lifeTime++;
            return true;
        }

        public abstract string Type { get; }
    }
}