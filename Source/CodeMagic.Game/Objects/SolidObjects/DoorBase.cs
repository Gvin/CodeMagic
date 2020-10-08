using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public abstract class DoorBase : WallBase, IUsableObject
    {
        private const string SaveKeyClosed = "Closed";

        protected DoorBase(SaveData data) : base(data)
        {
            Closed = data.GetBoolValue(SaveKeyClosed);
        }

        protected DoorBase(string name)
            : base(name)
        {
            Closed = true;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyClosed, Closed);
            return data;
        }

        protected bool Closed { get; private set; }

        public override bool BlocksMovement => Closed;

        public override bool BlocksAttack => Closed;

        public override bool BlocksProjectiles => Closed;

        public override bool BlocksVisibility => Closed;

        public override bool BlocksEnvironment => Closed;

        public bool CanUse => Closed;

        public void Use(GameCore<Player> game, Point position)
        {
            if (!Closed)
                return;

            Closed = false;
        }
    }
}