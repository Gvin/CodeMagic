using CodeMagic.Core.Game;

namespace CodeMagic.Core.Statuses
{
    public interface IObjectStatusesCollection
    {
        void Add(IObjectStatus status);

        TStatus[] GetStatuses<TStatus>() where TStatus : IObjectStatus;

        void Remove(string statusType);

        bool Contains(string statusType);

        void Update(Point position);
    }
}