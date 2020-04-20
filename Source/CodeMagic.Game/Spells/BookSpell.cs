using System.Collections.Generic;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects;

namespace CodeMagic.Game.Spells
{
    public class BookSpell : ISaveable
    {
        private const string SaveKeyName = "Name";
        private const string SaveKeyCode = "Code";
        private const string SaveKeyManaCost = "ManaCost";

        public BookSpell(SaveData data)
        {
            Name = data.GetStringValue(SaveKeyName);
            Code = data.GetStringValue(SaveKeyCode);
            ManaCost = data.GetIntValue(SaveKeyManaCost);
        }

        public BookSpell()
        {
        }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyName, Name},
                {SaveKeyCode, Code},
                {SaveKeyManaCost, ManaCost}
            });
        }

        public string Name { get; set; }

        public string Code { get; set; }

        public int ManaCost { get; set; }

        public CodeSpell CreateCodeSpell(ICreatureObject caster)
        {
            return new CodeSpell(caster, Name, Code, ManaCost);
        }
    }
}