using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects.PlayerData
{
    public class Player : CreatureObject, IPlayer, IDynamicObject
    {
        private int mana;
        private int maxMana;

        public Player(PlayerConfiguration configuration)
            : base(configuration)
        {
            MaxMana = configuration.MaxMana;
            Mana = configuration.Mana;
            ManaRegeneration = configuration.ManaRegeneration;

            Inventory = new Inventory(configuration.MaxWeight);
            Equipment = new Equipment();
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public event EventHandler Died;

        public Equipment Equipment { get; }

        public Inventory Inventory { get; }

        public override bool BlocksMovement => true;

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

        public int HitChance => CalculateHitChance(Equipment.Weapon.HitChance);

        public void Update(IGameCore game, Point position)
        {
            Mana += ManaRegeneration;
        }

        public bool Updated { get; set; }

        public override void OnDeath(IAreaMap map, Point position)
        {
            base.OnDeath(map, position);

            Died?.Invoke(this, EventArgs.Empty);
        }
    }
}
