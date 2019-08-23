using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Implementations.Items.Materials;

namespace CodeMagic.ItemsGeneration.Implementations
{
    public class ResourcesGenerator
    {
        public IItem GenerateResource()
        {
            var factory = RandomHelper.GetRandomElement(Resources);
            return factory();
        }

        private static readonly Func<IItem>[] Resources = 
        {
            () => new BlankScroll(),
        };
    }
}