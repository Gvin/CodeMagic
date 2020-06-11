using System;
using System.IO;
using CodeMagic.UI.Sad.Drawing;
using Newtonsoft.Json;

namespace CodeMagic.UI.Sad
{
    public class Settings
    {
        private const string SettingsFilePath = @"./Settings.json";

        public static Settings Current { get; }

        static Settings()
        {
            Current = LoadSettings();
        }

        private static Settings LoadSettings()
        {
            if (!File.Exists(SettingsFilePath))
            {
                return InitDefaultSettings();
            }

            try
            {
                var settingsString = File.ReadAllText(SettingsFilePath);
                var data = (SettingsObject) JsonConvert.DeserializeObject(settingsString, typeof(SettingsObject));
                return new Settings(data);
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#else
                return InitDefaultSettings();
#endif
            }
        }

        private static Settings InitDefaultSettings()
        {
            var data = new SettingsObject
            {
                Brightness = 0.17f,
                DebugDrawLightLevel = false,
                DebugDrawMagicEnergy = false,
                DebugDrawTemperature = false,
                SpellEditorPath = "notepad.exe",
                FontSize = FontSizeMultiplier.X1,
                DebugWriteMapToFile = true,
                LogLevel = "Debug",
                MinActionsInterval = 200,
                SavingInterval = 10
            };

            var result = new Settings(data);
            result.Save();
            return result;
        }

        private readonly SettingsObject data;

        private Settings(SettingsObject data)
        {
            this.data = data;
        }

        public float Brightness => data.Brightness;

        public bool DebugDrawTemperature => data.DebugDrawTemperature;

        public bool DebugDrawLightLevel => data.DebugDrawLightLevel;

        public bool DebugDrawMagicEnergy => data.DebugDrawMagicEnergy;

        public FontSizeMultiplier FontSize
        {
            get => data.FontSize;
            set => data.FontSize = value;
        }

        public string SpellEditorPath
        {
            get => data.SpellEditorPath;
            set => data.SpellEditorPath = value;
        }

        public int MinActionsInterval => data.MinActionsInterval;

        public int SavingInterval => data.SavingInterval;

        public bool DebugWriteMapToFile => data.DebugWriteMapToFile;

        public string LogLevel => data.LogLevel;

        public void Save()
        {
            var settingsString = JsonConvert.SerializeObject(data);
            File.WriteAllText(SettingsFilePath, settingsString);
        }

        public class SettingsObject
        {
            public float Brightness { get; set; }

            public bool DebugDrawTemperature { get; set; }

            public bool DebugDrawLightLevel { get; set; }

            public bool DebugDrawMagicEnergy { get; set; }

            public FontSizeMultiplier FontSize { get; set; }

            public string SpellEditorPath { get; set; }

            public int MinActionsInterval { get; set; }

            public int SavingInterval { get; set; }

            public bool DebugWriteMapToFile { get; set; }

            public string LogLevel { get; set; }
        }
    }
}