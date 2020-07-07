namespace CodeMagic.UI.Services
{
    public interface ISettingsService
    {
        float Brightness { get; }

        bool DebugDrawTemperature { get; }

        bool DebugDrawLightLevel { get; }

        bool DebugDrawMagicEnergy { get; }

        FontSizeMultiplier FontSize { get; set; }

        string SpellEditorPath { get; set; }

        int MinActionsInterval { get; }

        int SavingInterval { get; }

        bool DebugWriteMapToFile { get; }

        string LogLevel { get; }

        void Save();
    }

    public enum FontSizeMultiplier
    {
        X1,
        X2
    }
}