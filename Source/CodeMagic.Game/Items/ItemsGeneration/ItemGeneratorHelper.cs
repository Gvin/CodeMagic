using System;
using System.Linq;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.Items.ItemsGeneration
{
    public static class ItemGeneratorHelper
    {
        private static readonly Element[] BlacklistedElements = {
            Element.Magic
        };

        public static Element GetRandomDamageElement()
        {
            var elements = Enum.GetValues(typeof(Element)).Cast<Element>().Except(BlacklistedElements).ToArray();
            return RandomHelper.GetRandomElement(elements);
        }
    }
}