using CodeMagic.Core.Game;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.Objects
{
    public interface IResourceObject
    {
        void UseTool(IGameCore game, WeaponItem weapon, int damage, Element element);
    }
}