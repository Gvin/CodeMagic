using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Items;
using CodeMagic.Game.Statuses;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Creatures
{
    public class Player : CreatureObject, IPlayer, ILightObject, IWorldImageProvider
    {
        private const string ImageUp = "Player_Up";
        private const string ImageDown = "Player_Down";
        private const string ImageLeft = "Player_Left";
        private const string ImageRight = "Player_Right";

        private const int DefaultStatValue = 1;

        private const int MaxProtection = 75;

        private int mana;
        private int maxMana;
        private int manaRegeneration;
        private double hungerPercent;
        private readonly double hungerIncrement;
        private readonly Dictionary<PlayerStats, int> stats;

        public Player() : base(GetMaxHealth(DefaultStatValue))
        {
            Equipment = new Equipment();
            MaxMana = 1000;
            Mana = 1000;
            manaRegeneration = 10;
            MaxCarryWeight = 25000;
            hungerIncrement = 0.02;

            Inventory = new Inventory();
            Inventory.ItemRemoved += Inventory_ItemRemoved;

            stats = new Dictionary<PlayerStats, int>();
            foreach (var playerStat in Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>())
            {
                stats.Add(playerStat, DefaultStatValue);
            }
        }

        private static int GetMaxHealth(int strength)
        {
            return 100 + (strength - DefaultStatValue) * 10;
        }

        public int GetStat(PlayerStats stat)
        {
            return stats[stat];
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

        public int HungerPercent
        {
            get => (int)Math.Floor(hungerPercent);
            set => hungerPercent = Math.Min(100d, value);
        }

        public int MaxCarryWeight { get; }

        public event EventHandler Died;

        public Equipment Equipment { get; }

        public Inventory Inventory { get; }

        public override string Name => "Player";

        public override int MaxVisibilityRange => 4;

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

        public override int MaxHealth => GetMaxHealth(stats[PlayerStats.Strength]) + Equipment.GetBonusHealth();

        public int Mana
        {
            get => mana;
            set => mana = Math.Max(0, Math.Min(MaxMana, value));
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

        public override void Update(IAreaMap map, IJournal journal, Point position)
        {
            if (Mana < MaxMana && !Statuses.Contains(HungryObjectStatus.StatusType))
            {
                var cell = map.GetCell(position);
                var manaToRegenerate = Math.Min(ManaRegeneration, cell.MagicEnergyLevel());
                cell.Environment.Cast().MagicEnergyLevel -= manaToRegenerate;
                Mana += manaToRegenerate;
            }

            hungerPercent = Math.Min(100d, hungerPercent + hungerIncrement);
            if (hungerPercent >= 100d)
            {
                Statuses.Add(new HungryObjectStatus(), journal);
            }

            var weight = Inventory.GetWeight();
            if (weight > MaxCarryWeight)
            {
                Statuses.Add(new OverweightObjectStatus(), journal);
            }
        }

        public override void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
            base.OnDeath(map, journal, position);

            Died?.Invoke(this, EventArgs.Empty);
        }

        public override ObjectSize Size => ObjectSize.Medium;

        public override int GetProtection(Element element)
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

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            switch (Direction)
            {
                case Direction.North:
                    return storage.GetImage(ImageUp);
                case Direction.South:
                    return storage.GetImage(ImageDown);
                case Direction.West:
                    return storage.GetImage(ImageLeft);
                case Direction.East:
                    return storage.GetImage(ImageRight);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}