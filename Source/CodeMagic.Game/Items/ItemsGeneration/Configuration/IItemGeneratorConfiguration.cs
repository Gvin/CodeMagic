using CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.SpellBook;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration
{
    public interface IItemGeneratorConfiguration
    {
        IWeaponConfiguration WeaponConfiguration { get; }

        IArmorConfiguration ArmorConfiguration { get; }

        ISpellBooksConfiguration SpellBooksConfiguration { get; }

        IBonusesConfiguration BonusesConfiguration { get; }
    }
}