using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Statuses
{
    public class ObjectStatusesCollection : IObjectStatusesCollection
    {
        private readonly Dictionary<string, IObjectStatus> statuses;
        private readonly IDestroyableObject owner;

        public ObjectStatusesCollection(IDestroyableObject owner)
        {
            statuses = new Dictionary<string, IObjectStatus>();
            this.owner = owner;
        }

        public void Add(IObjectStatus status)
        {
            if (statuses.ContainsKey(status.Type))
            {
                statuses[status.Type] = status;
            }
            else
            {
                CurrentGame.Journal.Write(new StatusAddedMessage(owner, status.Type));
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
                var keepStatus = status.Update(owner, position);
                if (!keepStatus)
                {
                    statuses.Remove(status.Type);
                }
            }
        }
    }
}