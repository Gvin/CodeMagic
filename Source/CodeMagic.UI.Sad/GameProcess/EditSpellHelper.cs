using System.Diagnostics;
using System.IO;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Sad.Properties;

namespace CodeMagic.UI.Sad.GameProcess
{
    public static class EditSpellHelper
    {
        private const string TemplateSpellFilePath = @".\Resources\Templates\SpellTemplate.js";
        private const string SpellFileExtension = ".js";

        public static void LaunchSpellFileEditor(string filePath)
        {
            var editorPath = Settings.Default.SpellEditorPath;
            Process.Start(editorPath, filePath);
        }

        public static string ReadSpellCodeFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public static string PrepareSpellTemplate(BookSpell initialSpell)
        {
            var filePath = GenerateSpellFileName();
            if (initialSpell == null)
            {
                WriteTemplateDataToFile(filePath);
            }
            else
            {
                WriteSpellDataToFile(initialSpell, filePath);
            }

            return filePath;
        }

        private static void WriteSpellDataToFile(BookSpell spell, string filePath)
        {
            File.WriteAllText(filePath, spell.Code);
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