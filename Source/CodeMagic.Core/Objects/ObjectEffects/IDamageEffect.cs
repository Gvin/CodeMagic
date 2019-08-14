using CodeMagic.Core.Game;
using CodeMagic.Core.Injection;

namespace CodeMagic.Core.Objects.ObjectEffects
{
    public interface IDamageEffect : IObjectEffect, IInjectable
    {
        int Value { get; }

        Element Element { get; }
    }
}