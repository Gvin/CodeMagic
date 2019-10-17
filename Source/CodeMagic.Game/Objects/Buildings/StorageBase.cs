using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Buildings
{
    public abstract class StorageBase : IMapObject, IUsableObject, IWorldImageProvider, IStorageBuilding
    {
        protected StorageBase()
        {
            Inventory = new Inventory();
        }

        public event EventHandler Opened;

        public Inventory Inventory { get; }

        public abstract string Name { get; }
        public abstract int MaxWeight { get; }
        public bool BlocksMovement => true;
        public bool BlocksProjectiles => false;
        public bool IsVisible => true;
        public bool BlocksVisibility => false;
        public bool BlocksAttack => true;
        public bool BlocksEnvironment => false;
        public ZIndex ZIndex => ZIndex.BigDecoration;
        public bool Equals(IMapObject other)
        {
            return ReferenceEquals(this, other);
        }

        public ObjectSize Size => ObjectSize.Huge;
        public void Use(IGameCore game, Point position)
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        public abstract SymbolsImage GetWorldImage(IImagesStorage storage);
    }
}