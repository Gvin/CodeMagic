using System.Diagnostics;
using System.IO;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class EditSpellService : IEditSpellService
    {
        private const string TemplateSpellFilePath = @".\Resources\Templates\SpellTemplate.js";
        private const string SpellFileExtension = ".js";

        public void LaunchSpellFileEditor(string filePath)
        {
            var editorPath = Settings.Current.SpellEditorPath;
            Process.Start(editorPath, filePath);
        }

        public string ReadSpellCodeFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public string PrepareSpellTemplate(string initialSpellCode)
        {
            var filePath = GenerateSpellFileName();
            if (string.IsNullOrEmpty(initialSpellCode))
            {
                WriteTemplateDataToFile(filePath);
            }
            else
            {
                WriteSpellDataToFile(initialSpellCode, filePath);
            }

            return filePath;
        }

        private static void WriteSpellDataToFile(string spellCode, string filePath)
        {
            File.WriteAllText(filePath, spellCode);
        }

        private static void WriteTemplateDataToFile(string filePath)
        {
            if (!File.Exists(TemplateSpellFilePath))
                throw new FileNotFoundException("Spell template file not found.", TemplateSpellFilePath);

            var templateContent = File.ReadAllText(TemplateSpellFilePath);
            File.WriteAllText(filePath, templateContent);
        }

        private static string GenerateSpellFileName()
        {
            var tempFile = Path.GetTempFileName();
            return $"{tempFile}{SpellFileExtension}";
        }
    }
}