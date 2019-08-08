using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects.PlayerData
{
    public class Player : CreatureObject, IPlayer, IDynamicObject
    {
        private const int MaxProtection = 75;

        private int mana;
        private int maxMana;
        private int manaRegeneration;

        public Player(PlayerConfiguration configuration)
            : base(configuration)
        {
            Equipment = new Equipment();
            MaxMana = configuration.MaxMana;
            Mana = configuration.Mana;
            manaRegeneration = configuration.ManaRegeneration;

            Inventory = new Inventory(configuration.MaxWeight);
        }

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public event EventHandler Died;

        public Equipment Equipment { get; }

        public Inventory Inventory { get; }

        public override bool BlocksMovement => true;

        public int ManaRegeneration
        {
            get => manaRegeneration + Equipment.GetBonusManaRegeneration();
            set
            {
                if (value < 0)
                {
                    manaRegeneration = 0;
                    return;
                }

                manaRegeneration = value;
            }
        }

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
                if (value > MaxMana)
                {
                    mana = MaxMana;
                    return;
                }
                mana = value;
            }
        }

        public override int MaxHealth
        {
            get => base.MaxHealth + Equipment.GetBonusHealth();
            set => base.MaxHealth = value;
        }

        public int MaxMana
        {
            get => maxMana + Equipment.GetBonusMana();
            set
            {
                if (value < 0)
                    throw new ArgumentException("Max Mana value cannot be < 0");
                maxMana = value;
                if (mana > MaxMana)
                    mana = MaxMana;
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

        protected override int GetProtection(Element element)
        {
            var value = base.GetProtection(element) + Equipment.GetProtection(element);
            if (value > MaxProtection)
                return MaxProtection;
            return value;
        }
    }
}
