namespace CodeMagic.UI.Services
{
    public interface IEditSpellService
    {
        void LaunchSpellFileEditor(string filePath);

        string ReadSpellCodeFromFile(string filePath);

        string PrepareSpellTemplate(string initialSpellCode);
    }
}