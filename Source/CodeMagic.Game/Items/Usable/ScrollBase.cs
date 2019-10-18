using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects;
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

        public virtual bool Use(IGameCore game)
        {
            if (!game.World.CurrentLocation.CanCast)
            {
                game.Journal.Write(new CastNotAllowedMessage());
                return true;
            }

            var codeSpell = new CodeSpellImpl(game.Player, SpellName, code, Mana);
            game.Map.AddObject(game.PlayerPosition, codeSpell);
            return false;
        }

        public virtual string GetSpellCode()
        {
            return code;
        }

        public abstract SymbolsImage GetWorldImage(IImagesStorage storage);

        public abstract SymbolsImage GetInventoryImage(IImagesStorage storage);

        public abstract StyledLine[] GetDescription(IPlayer player);
    }
}