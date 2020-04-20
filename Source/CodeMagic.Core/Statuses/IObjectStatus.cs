using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Statuses
{
    public interface IObjectStatus : ISaveable
    {
        bool Update(IDestroyableObject owner, Point position);

        string Type { get; }
    }
}