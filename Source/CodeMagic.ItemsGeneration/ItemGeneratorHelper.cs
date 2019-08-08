using System;
using System.Linq;
using CodeMagic.Core.Game;

namespace CodeMagic.ItemsGeneration
{
    public static class ItemGeneratorHelper
    {
        public static Element GetRandomDamageElement()
        {
            var elements = Enum.GetValues(typeof(Element)).Cast<Element>().ToArray();
            return RandomHelper.GetRandomElement(elements);
        }
    }
}