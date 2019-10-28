using CodeMagic.Core.Game;
using CodeMagic.Game.Items;

namespace CodeMagic.Game.Objects
{
    public interface IResourceObject
    {
        void UseTool(IGameCore game, WeaponItem weapon, int damage, Element element);
    }
}