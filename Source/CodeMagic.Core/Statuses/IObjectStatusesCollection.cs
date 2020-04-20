using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Statuses
{
    public interface IObjectStatusesCollection : ISaveable
    {
        void Add(IObjectStatus status);

        TStatus[] GetStatuses<TStatus>() where TStatus : IObjectStatus;

        void Remove(string statusType);

        bool Contains(string statusType);

        void Update(Point position);
    }
}