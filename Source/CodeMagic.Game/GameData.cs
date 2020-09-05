using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Items.Usable.Potions;

namespace CodeMagic.Game
{
    public class GameData : ISaveable
    {
        private static readonly ILog Log = LogManager.GetLog<GameData>();

        public static GameData Current { get; private set; }

        public static void Initialize(GameData data)
        {
            Log.Debug("Initializing GameData");
            Current = data;
        }

        private const string SaveKeyPotionsPattern = "PotionsPattern";

        public GameData()
        {
            PotionsPattern = new Dictionary<PotionColor, PotionType>();
            InitializePotionTypes();
        }

        public GameData(SaveData data)
        {
            PotionsPattern = data.GetObject<DictionarySaveable>(SaveKeyPotionsPattern).Data.ToDictionary(
                pair => (PotionColor)int.Parse((string)pair.Key),
                pair => (PotionType)int.Parse((string)pair.Value));
        }

        public SaveDataBuilder GetSaveData()
        {
            var data = new Dictionary<string, object>
            {
                {
                    SaveKeyPotionsPattern, new DictionarySaveable(PotionsPattern.ToDictionary(
                        pair => (object) (int) pair.Key,
                        pair => (object) (int) pair.Value))
                }
            };
            return new SaveDataBuilder(GetType(), data);
        }

        public Dictionary<PotionColor, PotionType> PotionsPattern { get; }

        private void InitializePotionTypes()
        {
            Log.Debug("Initializing potion types");
            PotionsPattern.Clear();

            var colors = Enum.GetValues(typeof(PotionColor)).Cast<PotionColor>().ToList();
            var types = Enum.GetValues(typeof(PotionType)).Cast<PotionType>().ToList();

            foreach (var potionColor in colors)
            {
                var type = RandomHelper.GetRandomElement(types.ToArray());
                types.Remove(type);
                PotionsPattern.Add(potionColor, type);
            }
        }
    }
}