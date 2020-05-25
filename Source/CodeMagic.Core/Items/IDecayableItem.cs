using System;

namespace CodeMagic.Core.Items
{
    public interface IDecayItem : IItem
    {
        void Update();

        event EventHandler Decayed;
    }
}