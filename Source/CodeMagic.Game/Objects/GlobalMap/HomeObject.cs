﻿using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.MapGeneration.Home;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.GlobalMap
{
    public class HomeObject : IMapObject, IUsableObject, IWorldImageProvider
    {
        private const string WorldImageName = "GlobalMap_Home";

        public string Name => "Home";

        public bool BlocksAttack => true;

        public bool BlocksMovement => true;

        public bool BlocksProjectiles => false;

        public bool IsVisible => true;

        public bool BlocksVisibility => false;

        public bool BlocksEnvironment => false;

        public ZIndex ZIndex => ZIndex.BigDecoration;

        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;

        public void Use(GameCore<Player> game, Point position)
        {
            game.World.TravelToLocation(game, HomeLocationMapGenerator.LocationId, game.Player.Direction);
        }

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }
    }
}