using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Statuses
{
    public interface IObjectStatusesCollection
    {
        void Add(IObjectStatus status, IJournal journal);

        TStatus[] GetStatuses<TStatus>() where TStatus : IObjectStatus;

        void Remove(string statusType);

        bool Contains(string statusType);

        void Update(IAreaMapCell cell, IJournal journal);
    }
}