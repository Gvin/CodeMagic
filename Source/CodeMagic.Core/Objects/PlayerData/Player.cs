using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Configuration.Buildings;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Core.Objects.PlayerData
{
    public class Player : CreatureObject, IPlayer, IDynamicObject, ILightObject
    {
        private const int MaxProtection = 75;

        private int mana;
        private int maxMana;
        private int manaRegeneration;

        private readonly List<IBuildingConfiguration> unlockedBuildings;

        public Player(PlayerConfiguration configuration)
            : base(configuration)
        {
            Equipment = new Equipment();
            MaxMana = configuration.MaxMana;
            Mana = configuration.Mana;
            manaRegeneration = configuration.ManaRegeneration;
            MaxCarryWeight = configuration.MaxCarryWeight;

            Inventory = new Inventory();
            Inventory.ItemRemoved += Inventory_ItemRemoved;

            unlockedBuildings = new List<IBuildingConfiguration>();
        }

        private void Inventory_ItemRemoved(object sender, ItemEventArgs e)
        {
            if (!(e.Item is IEquipableItem equipable))
                return;

            if (Equipment.IsEquiped(equipable))
            {
                Equipment.UnequipItem(equipable);
            }
        }

        public bool UnlockBuilding(IBuildingConfiguration building)
        {
            if (unlockedBuildings.Any(unlocked => unlocked.Type == building.Type))
                return false;

            unlockedBuildings.Add(building);
            return true;
        }

        public bool GetIfBuildingUnlocked(IBuildingConfiguration building)
        {
            return unlockedBuildings.Any(unlocked => unlocked.Type == building.Type);
        }

        public int MaxCarryWeight { get; }

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

        public void Update(IAreaMap map, IJournal journal, Point position)
        {
            if (Mana < MaxMana)
            {
                var cell = map.GetCell(position);
                var manaToRegenerate = Math.Min(ManaRegeneration, cell.MagicEnergyLevel);
                cell.MagicEnergyLevel -= manaToRegenerate;
                Mana += manaToRegenerate;
            }

            var weight = Inventory.GetWeight();
            if (weight > MaxCarryWeight)
            {
                Statuses.Add(new OverweightObjectStatus(), journal);
            }
        }

        public bool Updated { get; set; }

        public override void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
            base.OnDeath(map, journal, position);

            Died?.Invoke(this, EventArgs.Empty);
        }

        protected override int GetProtection(Element element)
        {
            var value = base.GetProtection(element) + Equipment.GetProtection(element);
            if (value > MaxProtection)
                return MaxProtection;
            return value;
        }

        public ILightSource[] LightSources
        {
            get
            {
                var result = new List<ILightSource>();
                result.AddRange(Equipment.Armor.Values.Where(item => item != null));
                if (Equipment.Weapon != null)
                {
                    result.Add(Equipment.Weapon);
                }

                if (Equipment.SpellBook != null)
                {
                    result.Add(Equipment.SpellBook);
                }

                return result.ToArray();
            }
        }
    }
}
