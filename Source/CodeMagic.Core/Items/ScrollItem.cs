﻿using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
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

        public virtual bool Use(IGameCore game)
        {
            if (!game.World.CurrentLocation.CanCast)
            {
                game.Journal.Write(new CastNotAllowedMessage());
                return true;
            }

            var codeSpell = Injector.Current.Create<ICodeSpell>(game.Player, SpellName, code, Mana);
            game.Map.AddObject(game.PlayerPosition, codeSpell);
            return false;
        }

        public virtual string GetSpellCode()
        {
            return code;
        }
    }

    public class ScrollItemConfiguration : ItemConfiguration
    {
        public ScrollItemConfiguration()
        {
            Weight = 300;
        }

        public string SpellName { get; set; }

        public string Code { get; set; }

        public int Mana { get; set; }
    }
}