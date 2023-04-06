using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;
using CodeMagic.Game;
using CodeMagic.Game.GameProcess;
using CodeMagic.Game.Objects.Creatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CodeMagic.UI.Mono.Saving
{
    public class SaveService : ISaveService
    {
        private const string SaveFilePath = ".\\save.json";

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<SaveService> _logger;

        public SaveService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveService>();
            _loggerFactory = loggerFactory;
        }

        public Task SaveGameAsync(IGameCore game, GameData gameData)
        {
            return Task.Run(() => SaveGame(game, gameData));
        }

        public void SaveGame(IGameCore game, GameData gameData)
        {
            var turn = game.CurrentTurn;

            _logger.LogDebug($"Saving started on turn {turn}");

            var dataSerializer = new JsonDataSerializer();
            SaveData gameDataToSave;
            SaveData dataData;
            using (PerformanceMeter.Start($"Saving_GetSaveData[{turn}]"))
            {
                var gameDataBuilder = game.GetSaveData();
                gameDataToSave = gameDataBuilder.ConvertRawData(dataSerializer);
                var dataDataBuilder = gameData.GetSaveData();
                dataData = dataDataBuilder.ConvertRawData(dataSerializer);
            }

            var gameSaveData = new GameSaveData
            {
                Version = GetGameVersion(),
                Game = gameDataToSave,
                Data = dataData
            };

            string json;
            using (PerformanceMeter.Start($"Saving_Serialization[{turn}]"))
            {
                json = JsonConvert.SerializeObject(gameSaveData);
            }

            using (PerformanceMeter.Start($"Saving_Writing[{turn}]"))
            {
                WriteSaveFile(json);
            }

            _logger.LogDebug("Saving finished");
        }

        public (GameCore<Player>, GameData) LoadGame()
        {
            _logger.LogDebug("Trying to load game");

            if (!File.Exists(SaveFilePath))
                return (null, null);

            var stringData = ReadSaveFile();
            try
            {
                var gameSaveData = JsonConvert.DeserializeObject(stringData, typeof(GameSaveData)) as GameSaveData;
                if (gameSaveData == null)
                    return (null, null);

                var currentVersion = GetGameVersion();
                if (!string.Equals(currentVersion, gameSaveData.Version))
                {
                    _logger.LogWarning($"Throwing away existing save because of versions mismatch. Game version: {currentVersion}. Save version: {gameSaveData.Version}.");
                    return (null, null);
                }

                var game = new GameCore<Player>(gameSaveData.Game, _loggerFactory.CreateLogger<GameCore<Player>>());
                var data = new GameData(gameSaveData.Data);
                return (game, data);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error while loading save file", ex);
#if DEBUG
                throw;
#else
                return (null, null);
#endif
            }
            finally
            {
                _logger.LogDebug("Loading finished");
            }
        }

        public void DeleteSave()
        {
            _logger.LogDebug("Deleting game save");
            File.Delete(SaveFilePath);
        }

        private static void WriteSaveFile(string data)
        {
            // TODO: Add save file content encryption
            File.WriteAllText(SaveFilePath, data);
        }

        private static string ReadSaveFile()
        {
            return File.ReadAllText(SaveFilePath);
        }

        private string GetGameVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private class JsonDataSerializer : IDataSerializer
        {
            public string Serialize(object data)
            {
                return JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }

            public T Deserialize<T>(string dataString) where T : class
            {
                return JsonConvert.DeserializeObject<T>(dataString);
            }
        }
    }

    public class GameSaveData
    {
        public string Version { get; set; }

        public SaveData Game { get; set; }

        public SaveData Data { get; set; }
    }
}