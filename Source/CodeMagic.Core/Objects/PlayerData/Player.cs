using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Objects.PlayerData
{
    public class Player : DestroyableObject, IPlayer, IDynamicObject
    {
        private int mana;
        private int maxMana;

        public Player(PlayerConfiguration configuration)
            : base(configuration)
        {
            MaxMana = configuration.MaxMana;
            Mana = configuration.Mana;
            ManaRegeneration = configuration.ManaRegeneration;
            MaxVisibilityRange = configuration.VisibilityRange;

            Inventory = new Inventory(configuration.MaxWeight);
            Equipment = new Equipment();

            Direction = Direction.Up;
        }

        public event EventHandler Died;

        public Equipment Equipment { get; }

        public Inventory Inventory { get; }

        public override bool BlocksMovement => true;

        public Direction Direction { get; set; }

        public int VisibilityRange
        {
            get
            {
                if (Statuses.Contains(BlindObjectStatus.StatusType))
                    return 0;

                return MaxVisibilityRange;
            }
        }

        public int MaxVisibilityRange { get; }

        public int ManaRegeneration { get; set; }

        public int Mana
        {
            get => mana;
            set
            {
                if (value < 0)
                {
                    mana = 0;
                    return;
                }
                if (value > maxMana)
                {
                    mana = maxMana;
                    return;
                }
                mana = value;
            }
        }

        public int MaxMana
        {
            get => maxMana;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Max Mana value cannot be < 0");
                maxMana = value;
                if (mana > maxMana)
                    mana = maxMana;
            }
        }

        public void Update(IGameCore game, Point position)
        {
            Mana += ManaRegeneration;

            var cell = game.Map.GetCell(position);
            if (cell.LightLevel == LightLevel.Blinding)
            {
                Statuses.Add(new BlindObjectStatus());
            }
        }

        public bool Updated { get; set; }

        public override void OnDeath(IAreaMap map, Point position)
        {
            base.OnDeath(map, position);

            Died?.Invoke(this, EventArgs.Empty);
        }
    }
}
