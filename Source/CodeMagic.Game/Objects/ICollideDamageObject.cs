using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Objects
{
    public interface ICollideDamageObject
    {
        void Damage(IDestroyableObject collidedObject, Point position);
    }
}