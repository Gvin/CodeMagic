using System;
using CodeMagic.Core.Game;

namespace CodeMagic.Implementations.Items
{
    public static class ElementNameHelper
    {
        public static string GetElementName(Element element)
        {
            switch (element)
            {
                case Element.Blunt:
                    return "Blunt";
                case Element.Slashing:
                    return "Slashing";
                case Element.Piercing:
                    return "Piercing";
                case Element.Fire:
                    return "Fire";
                case Element.Frost:
                    return "Frost";
                case Element.Acid:
                    return "Acid";
                case Element.Electricity:
                    return "Electricity";
                default:
                    throw new ArgumentException($"Unknown element: {element}");
            }
        }
    }
}