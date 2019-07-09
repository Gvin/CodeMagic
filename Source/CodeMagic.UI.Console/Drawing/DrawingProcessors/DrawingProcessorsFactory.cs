using System;
using System.Collections.Generic;
using CodeMagic.Core.Objects.Creatures.Implementations;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Core.Objects.StationaryObjects;
using CodeMagic.Core.Spells;
using CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation;
using CodeMagic.UI.Console.Drawing.DrawingProcessors.ProcessorsImplementation.Monsters;

namespace CodeMagic.UI.Console.Drawing.DrawingProcessors
{
    public class DrawingProcessorsFactory : IDrawingProcessorsFactory
    {
        private readonly Dictionary<Type, IDrawingProcessor> mapping;

        public DrawingProcessorsFactory()
        {
            mapping = new Dictionary<Type, IDrawingProcessor>
            {
                {typeof(Player), new PlayerDrawingProcessor()},
                {typeof(StationaryObject), new StationaryObjectDrawingProcessor()},
                {typeof(DecorativeObject), new DecorativeObjectDrawingProcessor()},
                {typeof(SolidObject), new SolidObjectDrawingProcessor()},
                {typeof(CodeSpell), new CodeSpellDrawingProcessor()},
                {typeof(EnergyWall), new EnergyWallDrawingProcessor()},
                {typeof(FireDecorativeObject), new FireDecorativeObjectDrawingProcessor()},
                // Creatures
                {typeof(GoblinCreatureObject), new GoblinDrawingProcessor()}
            };
        }

        public IDrawingProcessor GetProcessor(object @object)
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            var type = @object.GetType();
            if (mapping.ContainsKey(type))
                return mapping[type];

            return null;
        }

    }
}