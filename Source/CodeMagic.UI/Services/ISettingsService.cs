namespace CodeMagic.UI.Services
{
    public interface ISettingsService
    {
        public float Brightness { get; }

        public bool DebugDrawTemperature { get; }

        public bool DebugDrawLightLevel { get; }

        public bool DebugDrawMagicEnergy { get; }

        public FontSizeMultiplier FontSize { get; set; }

        public string SpellEditorPath { get; set; }

        public int MinActionsInterval { get; }

        public int SavingInterval { get; }

        public bool DebugWriteMapToFile { get; }

        public string LogLevel { get; }

        public void Save();
    }

    public enum FontSizeMultiplier
    {
        X1,
        X2
    }
}