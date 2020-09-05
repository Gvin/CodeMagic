﻿using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.Core.Saving;
using CodeMagic.Game;
using CodeMagic.Game.GameProcess;
using CodeMagic.Game.Objects.Creatures;
using Newtonsoft.Json;

namespace CodeMagic.UI.Sad.Saving
{
    public class SaveService : ISaveService
    {
        private static readonly ILog Log = LogManager.GetLog<SaveService>();

        private const string SaveFilePath = ".\\save.json";

        public Task SaveGameAsync()
        {
            return Task.Run(SaveGame);
        }

        public void SaveGame()
        {
            var turn = CurrentGame.Game.CurrentTurn;

            Log.Debug($"Saving started on turn {turn}");

            SaveData gameData;
            SaveData dataData;
            using (PerformanceMeter.Start($"Saving_GetSaveData[{turn}]"))
            {
                var gameDataBuilder = CurrentGame.Game.GetSaveData();
                gameData = gameDataBuilder.ConvertRawData(new JsonDataSerializer());
                var dataDataBuilder = GameData.Current.GetSaveData();
                dataData = dataDataBuilder.ConvertRawData(new JsonDataSerializer());
            }

            var gameSaveData = new GameSaveData
            {
                Version = GetGameVersion(),
                Game = gameData,
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

            Log.Debug("Saving finished");
        }

        public (GameCore<Player>, GameData) LoadGame()
        {
            Log.Debug("Trying to load game");

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
                    Log.Warning($"Throwing away existing save because of versions mismatch. Game version: {currentVersion}. Save version: {gameSaveData.Version}.");
                    return (null, null);
                }

                var game = new GameCore<Player>(gameSaveData.Game);
                var data = new GameData(gameSaveData.Data);
                return (game, data);
            }
            catch (Exception ex)
            {
                Log.Warning("Error while loading save file", ex);
#if DEBUG
                throw;
#else
                return (null, null);
#endif
            }
            finally
            {
                Log.Debug("Loading finished");
            }
        }

        public void DeleteSave()
        {
            Log.Debug("Deleting game save");
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