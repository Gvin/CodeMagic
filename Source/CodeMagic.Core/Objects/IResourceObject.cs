using CodeMagic.Core.Game;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Objects
{
    public interface IResourceObject
    {
        void UseTool(IGameCore game, WeaponItem weapon, int damage, Element element);
    }
}