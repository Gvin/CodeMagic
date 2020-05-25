using CodeMagic.Core.Game;

namespace CodeMagic.Game.Items
{
    public interface IArmorItem : IEquipableItem
    {
        ArmorType ArmorType { get; }

        int GetProtection(Element element);
    }
}