using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.DecorativeObjects;
using CodeMagic.Game.Statuses;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Creatures
{
    public class Player : CreatureObject, IPlayer, ILightObject, IWorldImageProvider
    {
        private const string SaveKeyInventory = "Inventory";
        private const string SaveKeyEquipment = "Equipment";
        private const string SaveKeyStats = "Stats";
        private const string SaveKeyMana = "Mana";
        private const string SaveKeyHunger = "Hunger";
        private const string SaveKeyExperience = "Experience";

        private const string ImageUp = "Player_Up";
        private const string ImageDown = "Player_Down";
        private const string ImageLeft = "Player_Left";
        private const string ImageRight = "Player_Right";

        private const int DefaultStatValue = 1;

        private const int MaxProtection = 75;

        private int mana;
        private double hungerPercent;
        private readonly double hungerIncrement = 0.02;
        private readonly Dictionary<PlayerStats, int> stats;
        private int experience;

        public Player(SaveData data) : base(data)
        {
            Inventory = data.GetObject<Inventory>(SaveKeyInventory);
            Inventory.ItemRemoved += Inventory_ItemRemoved;

            var equipmentData = data.Objects[SaveKeyEquipment];
            Equipment = new Equipment(equipmentData, Inventory);

            stats = data.GetObject<DictionarySaveable>(SaveKeyStats).Data.ToDictionary(
                pair => (PlayerStats) int.Parse((string)pair.Key),
                pair => int.Parse((string) pair.Value));

            Mana = data.GetIntValue(SaveKeyMana);
            hungerPercent = double.Parse(data.GetStringValue(SaveKeyHunger));
            experience = data.GetIntValue(SaveKeyExperience);
        }

        public Player() : base("Player", GetMaxHealth(DefaultStatValue))
        {
            Equipment = new Equipment();

            Inventory = new Inventory();
            Inventory.ItemRemoved += Inventory_ItemRemoved;

            stats = new Dictionary<PlayerStats, int>();
            foreach (var playerStat in Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>())
            {
                stats.Add(playerStat, DefaultStatValue);
            }

            Mana = MaxMana;
            hungerPercent = 0;
            experience = 0;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyEquipment, Equipment);
            data.Add(SaveKeyInventory, Inventory);
            data.Add(SaveKeyStats, new DictionarySaveable(stats.ToDictionary(pair => (object)(int)pair.Key, pair => (object)pair.Value)));
            data.Add(SaveKeyMana, Mana);
            data.Add(SaveKeyHunger, hungerPercent);
            data.Add(SaveKeyExperience, experience);
            return data;
        }

        public int Experience => experience;

        public void AddExperience(int exp)
        {
            experience += exp;
            CurrentGame.Journal.Write(new ExperienceGainedMessage(exp));
        }

        private static int GetMaxHealth(int strength)
        {
            return 10 + strength * 10;
        }

        private int GetStat(PlayerStats stat)
        {
            return GetPureStat(stat) + Equipment.GetStatsBonus(stat);
        }

        public int GetPureStat(PlayerStats stat)
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

        public int MaxCarryWeight => 23000 + 2000 * GetStat(PlayerStats.Strength);

        public event EventHandler Died;

        public Equipment Equipment { get; }

        public Inventory Inventory { get; }

        public override int MaxVisibilityRange => 4;

        protected override IMapObject GenerateDamageMark()
        {
            return new CreatureRemains(RemainsType.BloodRedSmall);
        }

        public override bool BlocksMovement => true;

        public int ManaRegeneration => 2 + GetStat(PlayerStats.Wisdom) + Equipment.GetBonus(EquipableBonusType.ManaRegeneration);

        public override int MaxHealth => GetMaxHealth(GetStat(PlayerStats.Strength)) + Equipment.GetBonus(EquipableBonusType.Health);

        public int Mana
        {
            get => mana;
            set => mana = Math.Max(0, Math.Min(MaxMana, value));
        }

        public int MaxMana => 100 + 20 * GetStat(PlayerStats.Intelligence) + Equipment.GetBonus(EquipableBonusType.Mana);

        public int HitChance => CalculateHitChance(Equipment.Weapon.HitChance);

        public override void Update(Point position)
        {
            base.Update(position);

            if (Mana < MaxMana && !Statuses.Contains(HungryObjectStatus.StatusType))
            {
                var cell = CurrentGame.Map.GetCell(position);
                var manaToRegenerate = Math.Min(ManaRegeneration, cell.MagicEnergyLevel());
                cell.Environment.Cast().MagicEnergyLevel -= manaToRegenerate;
                Mana += manaToRegenerate;
            }

            hungerPercent = Math.Min(100d, hungerPercent + hungerIncrement);
            if (hungerPercent >= 100d)
            {
                Statuses.Add(new HungryObjectStatus());
            }

            var weight = Inventory.GetWeight();
            if (weight > MaxCarryWeight)
            {
                Statuses.Add(new OverweightObjectStatus());
            }
        }

        public override void OnDeath(Point position)
        {
            base.OnDeath(position);

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