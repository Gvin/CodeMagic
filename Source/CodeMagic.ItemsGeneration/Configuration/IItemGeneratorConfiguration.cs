using CodeMagic.ItemsGeneration.Configuration.Armor;
using CodeMagic.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.ItemsGeneration.Configuration.SpellBook;
using CodeMagic.ItemsGeneration.Configuration.Tool;
using CodeMagic.ItemsGeneration.Configuration.Weapon;

namespace CodeMagic.ItemsGeneration.Configuration
{
    public interface IItemGeneratorConfiguration
    {
        IWeaponConfiguration WeaponConfiguration { get; }

        IArmorConfiguration ArmorConfiguration { get; }

        ISpellBooksConfiguration SpellBooksConfiguration { get; }

        IBonusesConfiguration BonusesConfiguration { get; }

        IToolsConfiguration ToolsConfiguration { get; }
    }
}