using CodeMagic.Core.Game;
using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Items
{
    public abstract class ScrollItem : Item, IUsableItem
    {
        protected readonly string SpellName;
        private readonly string code;
        protected readonly int Mana;

        protected ScrollItem(ScrollItemConfiguration configuration) 
            : base(configuration)
        {
            SpellName = configuration.SpellName;
            code = configuration.Code;
            Mana = configuration.Mana;
        }

        public bool Use(IGameCore game)
        {
            var codeSpell = new CodeSpell(game.Player, SpellName, code, Mana);
            game.Map.AddObject(game.PlayerPosition, codeSpell);
            return false;
        }
    }

    public class ScrollItemConfiguration : ItemConfiguration
    {
        public string SpellName { get; set; }

        public string Code { get; set; }

        public int Mana { get; set; }
    }
}