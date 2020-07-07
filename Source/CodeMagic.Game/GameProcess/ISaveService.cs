using System.Threading.Tasks;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.GameProcess
{
    public interface ISaveService
    {
        void SaveGame();

        (GameCore<Player>, GameData) LoadGame();

        Task SaveGameAsync();

        void DeleteSave();
    }
}