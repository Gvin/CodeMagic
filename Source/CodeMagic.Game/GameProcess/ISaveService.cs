using System.Threading.Tasks;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.GameProcess
{
    public interface ISaveService
    {
        void SaveGame(IGameCore game, GameData gameData);

        (GameCore<Player>, GameData) LoadGame();

        Task SaveGameAsync(IGameCore game, GameData gameData);

        void DeleteSave();
    }
}