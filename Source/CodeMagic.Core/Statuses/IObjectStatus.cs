using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public interface IObjectStatus
    {
        bool Update(IDestroyableObject owner, AreaMapCell cell, Journal journal);

        string Type { get; }
    }
}