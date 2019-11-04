﻿using CodeMagic.Core.Common;

namespace CodeMagic.Core.Objects.Creatures
{
    public interface ICreatureObject : IDestroyableObject, IDynamicObject
    {
        Direction Direction { get; set; }

        int VisibilityRange { get; }
    }
}