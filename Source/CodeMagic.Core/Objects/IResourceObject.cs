﻿using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Objects
{
    public interface IResourceObject
    {
        void UseTool(WeaponItem weapon, IJournal journal, int damage, Element element);
    }
}