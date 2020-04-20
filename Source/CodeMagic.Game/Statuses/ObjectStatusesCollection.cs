using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Statuses
{
    public class ObjectStatusesCollection : IObjectStatusesCollection
    {
        private const string SaveKeyOwnerId = "OwnerId";
        private const string SaveKeyStatuses = "Statuses";

        private readonly Dictionary<string, IObjectStatus> statuses;
        private readonly string ownerId;

        public ObjectStatusesCollection(SaveData data)
        {
            ownerId = data.GetStringValue(SaveKeyOwnerId);
            statuses = data.GetObjectsCollection<IObjectStatus>(SaveKeyStatuses).ToDictionary(status => status.Type, status => status);
        }

        public ObjectStatusesCollection(string ownerId)
        {
            statuses = new Dictionary<string, IObjectStatus>();
            this.ownerId = ownerId;
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyOwnerId, ownerId},
                {SaveKeyStatuses, statuses.Values}
            });
        }

        public void Add(IObjectStatus status)
        {
            if (statuses.ContainsKey(status.Type))
            {
                statuses[status.Type] = status;
            }
            else
            {
                CurrentGame.Journal.Write(new StatusAddedMessage(CurrentGame.Map.GetDestroyableObject(ownerId), status.Type));
                statuses.Add(status.Type, status);
            }
        }

        public TStatus[] GetStatuses<TStatus>() where TStatus : IObjectStatus
        {
            return statuses.Values.OfType<TStatus>().ToArray();
        }

        public TStatus GetStatus<TStatus>()
        {
            return statuses.Values.OfType<TStatus>().FirstOrDefault();
        }

        public void Remove(string statusType)
        {
            if (statuses.ContainsKey(statusType))
                statuses.Remove(statusType);
        }

        public bool Contains(string statusType)
        {
            return statuses.ContainsKey(statusType);
        }

        public void Update(Point position)
        {
            foreach (var status in statuses.Values.ToArray())
            {
                var keepStatus = status.Update(CurrentGame.Map.GetDestroyableObject(ownerId), position);
                if (!keepStatus)
                {
                    statuses.Remove(status.Type);
                }
            }
        }
    }
}