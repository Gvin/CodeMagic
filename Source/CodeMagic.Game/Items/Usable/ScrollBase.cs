using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public abstract class ScrollBase : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        private const string SaveKeySpellName = "SpellName";
        private const string SaveKeyCode = "Code";
        private const string SaveKeyMana = "Mana";

        protected readonly string SpellName;
        private readonly string code;
        protected readonly int Mana;

        protected ScrollBase(SaveData data) : base(data)
        {
            SpellName = data.GetStringValue(SaveKeySpellName);
            code = data.GetStringValue(SaveKeyCode);
            Mana = data.GetIntValue(SaveKeyMana);
        }

        protected ScrollBase(ScrollItemConfiguration configuration)
            : base(configuration)
        {
            SpellName = configuration.SpellName;
            code = configuration.Code;
            Mana = configuration.Mana;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeySpellName, SpellName);
            data.Add(SaveKeyCode, code);
            data.Add(SaveKeyMana, Mana);
            return data;
        }

        public virtual bool Use(GameCore<Player> game)
        {
            var codeSpell = new CodeSpell(game.Player, SpellName, code, Mana);
            game.Map.AddObject(game.PlayerPosition, codeSpell);
            return false;
        }

        public virtual string GetSpellCode()
        {
            return code;
        }

        public abstract SymbolsImage GetWorldImage(IImagesStorage storage);

        public abstract SymbolsImage GetInventoryImage(IImagesStorage storage);

        public abstract StyledLine[] GetDescription(Player player);
    }
}