using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Statuses
{
    public abstract class PassiveObjectStatusBase : IObjectStatus
    {
        private const string SaveDataTimeToLive = "TimeToLive";

        private int timeToLive;

        protected PassiveObjectStatusBase(SaveData data)
        {
            timeToLive = data.GetIntValue(SaveDataTimeToLive);
        }

        protected PassiveObjectStatusBase(int timeToLive)
        {
            this.timeToLive = timeToLive;
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveDataTimeToLive, timeToLive}
            });
        }

        public bool Update(IDestroyableObject owner, Point position)
        {
            if (timeToLive <= 0)
            {
                return false;
            }

            timeToLive--;
            return true;
        }

        public IObjectStatus Merge(IObjectStatus oldStatus)
        {
            if (!(oldStatus is PassiveObjectStatusBase passiveStatus) || !string.Equals(oldStatus.Type, Type))
                throw new InvalidOperationException($"Unable to merge {GetType().Name} status with {oldStatus.GetType().Name}");

            if (passiveStatus.timeToLive > timeToLive)
                return passiveStatus;
            return this;
        }

        public abstract string Type { get; }
    }
}