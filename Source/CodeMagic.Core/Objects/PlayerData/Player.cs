using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Objects.PlayerData
{
    public class Player : DestroyableObject, IPlayer, IDynamicObject
    {
        private int mana;
        private int maxMana;
        private int visionRange;

        public Player(PlayerConfiguration configuration)
            : base(configuration)
        {
            MaxMana = configuration.MaxMana;
            Mana = configuration.Mana;
            ManaRegeneration = configuration.ManaRegeneration;
            visionRange = configuration.VisionRange;

            Inventory = new Inventory(configuration.MaxWeight);
            Equipment = new Equipment();
        }

        public Equipment Equipment { get; }

        public Inventory Inventory { get; }

        public override bool BlocksMovement => true;

        public Direction Direction { get; set; }

        public int VisionRange
        {
            get => visionRange;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"Vision range cannot be less than 0. Got {value}", nameof(value));
                visionRange = value;
            }
        }

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

        public void Update(IAreaMap map, Point position, Journal journal)
        {
            Mana += ManaRegeneration;
        }

        public bool Updated { get; set; }
    }
}
