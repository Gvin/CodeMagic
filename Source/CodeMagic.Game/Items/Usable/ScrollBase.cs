using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable
{
    public abstract class ScrollBase : Item, IUsableItem, IWorldImageProvider, IInventoryImageProvider, IDescriptionProvider
    {
        
        protected readonly string SpellName;
        private readonly string code;
        protected readonly int Mana;

        protected ScrollBase(ScrollItemConfiguration configuration)
            : base(configuration)
        {
            SpellName = configuration.SpellName;
            code = configuration.Code;
            Mana = configuration.Mana;
        }

        public virtual bool Use(GameCore<Player> game)
        {
            if (!game.World.CurrentLocation.CanCast)
            {
                game.Journal.Write(new CastNotAllowedMessage());
                return true;
            }

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