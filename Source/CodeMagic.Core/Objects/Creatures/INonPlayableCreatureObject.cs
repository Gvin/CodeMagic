﻿using CodeMagic.Core.Game;

namespace CodeMagic.Core.Objects.Creatures
{
    public interface INonPlayableCreatureObject : ICreatureObject
    {
        void Attack(Point position, Point targetPosition, IDestroyableObject target);
    }
}