using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Logging;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Usable.Potions;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.DecorativeObjects;
using CodeMagic.Game.Statuses;
using CodeMagic.UI.Images;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Objects.Creatures
{
    public class Player : CreatureObject, IPlayer, ILightObject, IWorldImageProvider
    {
        private static readonly ILog Log = LogManager.GetLog<Player>();

        private const string SaveKeyInventory = "Inventory";
        private const string SaveKeyEquipment = "Equipment";
        private const string SaveKeyStats = "Stats";
        private const string SaveKeyMana = "Mana";
        private const string SaveKeyStamina = "Stamina";
        private const string SaveKeyHunger = "Hunger";
        private const string SaveKeyExperience = "Experience";
        private const string SaveKeyLevel = "Level";
        private const string SaveKeyKnownPotions = "KnownPotions";
        private const string SaveKeyRegeneration = "Regeneration";

        private const string ImageUp = "Creature_Up";
        private const string ImageDown = "Creature_Down";
        private const string ImageLeft = "Creature_Left";
        private const string ImageRight = "Creature_Right";

        private const string ImageUpUsable = "Player_Up_Usable";
        private const string ImageDownUsable = "Player_Down_Usable";
        private const string ImageLeftUsable = "Player_Left_Usable";
        private const string ImageRightUsable = "Player_Right_Usable";

        private const string ImageBody = "Player_Body";
        private const string ImageBodyLeftWeapon = "Player_Body_LeftWeapon";
        private const string ImageBodyRightWeapon = "Player_Body_RightWeapon";

        private const int DefaultStatValue = 1;

        private const int MaxProtection = 75;
        private const int MaxDodgeChance = 50;

        private const double HungerIncrement = 0.02;
        private const double RegenerationIncrement = 0.03;

        private const double HungerBlocksRegeneration = 30d;
        private const double HungerBlocksManaRestore = 70d;

        private int mana;
        private int stamina;
        private double regeneration;
        private double hungerPercent;
        private readonly Dictionary<PlayerStats, int> stats;

        public event EventHandler Died;
        public event EventHandler LeveledUp;

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
            Stamina = data.GetIntValue(SaveKeyStamina);

            hungerPercent = double.Parse(data.GetStringValue(SaveKeyHunger));
            regeneration = double.Parse(data.GetStringValue(SaveKeyRegeneration));
            Experience = data.GetIntValue(SaveKeyExperience);
            Level = data.GetIntValue(SaveKeyLevel);

            KnownPotions = data.GetValuesCollection(SaveKeyKnownPotions).Select(value => (PotionType) int.Parse(value))
                .ToList();
        }

        public Player() : base("Player", GetMaxHealth(DefaultStatValue))
        {
            Equipment = new Equipment();

            Inventory = new Inventory();
            Inventory.ItemRemoved += Inventory_ItemRemoved;

            KnownPotions = new List<PotionType>();

            stats = new Dictionary<PlayerStats, int>();
            foreach (var playerStat in Enum.GetValues(typeof(PlayerStats)).Cast<PlayerStats>())
            {
                stats.Add(playerStat, DefaultStatValue);
            }

            Mana = MaxMana;
            Stamina = MaxStamina;
            hungerPercent = 0d;
            regeneration = 0d;
            Experience = 0;
            Level = 1;
        }

        protected override Dictionary<string, object> GetSaveDataContent()
        {
            var data = base.GetSaveDataContent();
            data.Add(SaveKeyEquipment, Equipment);
            data.Add(SaveKeyInventory, Inventory);
            data.Add(SaveKeyStats, new DictionarySaveable(stats.ToDictionary(pair => (object)(int)pair.Key, pair => (object)pair.Value)));
            data.Add(SaveKeyMana, Mana);
            data.Add(SaveKeyStamina, Stamina);
            data.Add(SaveKeyHunger, hungerPercent);
            data.Add(SaveKeyRegeneration, regeneration);
            data.Add(SaveKeyExperience, Experience);
            data.Add(SaveKeyLevel, Level);
            data.Add(SaveKeyKnownPotions, KnownPotions.Select(type => (int)type).ToArray());
            return data;
        }

        public List<PotionType> KnownPotions { get; }

        public int Experience { get; private set; }

        public int Level { get; private set; }

        public void AddExperience(int exp)
        {
            Experience += exp;
            CurrentGame.Journal.Write(new ExperienceGainedMessage(exp));

            var xpToLevelUp = GetXpToLevelUp();
            if (Experience >= xpToLevelUp)
            {
                Log.Debug($"Leveled Up. EXP: {Experience}, EXP to LVL: {GetXpToLevelUp()}");
                Level++;
                Experience -= xpToLevelUp;
                CurrentGame.Journal.Write(new LevelUpMessage(Level));
                LeveledUp?.Invoke(this, EventArgs.Empty);
            }
        }

        public void IncreaseStat(PlayerStats stat)
        {
            stats[stat]++;
        }

        public int GetXpToLevelUp()
        {
            var config = ConfigurationManager.Current.Levels;
            return (int)Math.Pow(Level, config.PlayerLevels.XpLevelPower) * config.PlayerLevels.XpMultiplier;
        }

        private static int GetMaxHealth(int endurance)
        {
            return 10 + endurance * 10;
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
            set => hungerPercent = Math.Max(0d, Math.Min(100d, value));
        }

        public int MaxCarryWeight => 23000 + 2000 * GetStat(PlayerStats.Strength);

        public Equipment Equipment { get; }

        public Inventory Inventory { get; }

        public override int MaxVisibilityRange => 4;

        public override int DodgeChance =>
            Math.Min(MaxDodgeChance, 1 * (GetStat(PlayerStats.Agility) - DefaultStatValue));

        public int DamageBonus => 2 * (GetStat(PlayerStats.Strength) - DefaultStatValue);

        public int AccuracyBonus => 1 * (GetStat(PlayerStats.Agility) - DefaultStatValue);

        public int ScrollReadingBonus => 2 * (GetStat(PlayerStats.Wisdom) - DefaultStatValue);

        protected override IMapObject GenerateDamageMark()
        {
            return new CreatureRemains(RemainsType.BloodRedSmall);
        }

        public override bool BlocksMovement => true;

        public int ManaRegeneration => 2 + GetStat(PlayerStats.Wisdom) + Equipment.GetBonus(EquipableBonusType.ManaRegeneration);

        public override int MaxHealth => GetMaxHealth(GetStat(PlayerStats.Endurance)) + Equipment.GetBonus(EquipableBonusType.Health);

        public int Stamina
        {
            get => stamina;
            set => stamina = Math.Max(0, Math.Min(MaxStamina, value));
        }

        public int MaxStamina =>
            80 + 20 * GetStat(PlayerStats.Endurance) + Equipment.GetBonus(EquipableBonusType.Stamina);

        public int Mana
        {
            get => mana;
            set => mana = Math.Max(0, Math.Min(MaxMana, value));
        }

        public int MaxMana => 80 + 20 * GetStat(PlayerStats.Intelligence) + Equipment.GetBonus(EquipableBonusType.Mana);

        public int AccuracyLeft => CalculateHitChance(Equipment.RightWeapon.Accuracy);

        public int AccuracyRight => CalculateHitChance(Equipment.RightWeapon.Accuracy);

        protected override void ApplyRealDamage(int damage, Element element, Point position)
        {
            base.ApplyRealDamage(damage, element, position);

            var targetArmorType = RandomHelper.GetRandomEnumValue<ArmorType>();
            if (Equipment.Armor[targetArmorType] is DurableItem targetArmor)
            {
                targetArmor.Durability--;
            }
        }

        public override void Update(Point position)
        {
            base.Update(position);

            Inventory.Update();

            IncrementHunger();
            RegenerateHealth();
            RegenerateMana(position);
            CheckOverweight();
        }

        private void RegenerateHealth()
        {
            if (hungerPercent >= HungerBlocksRegeneration)
            {
                regeneration = 0;
                return;
            }

            regeneration += RegenerationIncrement;
            if (regeneration >= 1d)
            {
                regeneration -= 1d;
                Health += 1;
            }

            if (Health == MaxHealth)
            {
                regeneration = 0;
            }
        }

        private void RegenerateMana(Point position)
        {
            if (hungerPercent >= HungerBlocksManaRestore)
                return;

            var manaRegeneration = ManaRegeneration;
            if (Statuses.Contains(ManaDisturbedObjectStatus.StatusType))
            {
                manaRegeneration = (int)Math.Floor(manaRegeneration / 2d);
            }

            if (Mana < MaxMana && !Statuses.Contains(HungryObjectStatus.StatusType))
            {
                var cell = CurrentGame.Map.GetCell(position);
                var manaToRegenerate = Math.Min(manaRegeneration, cell.MagicEnergyLevel());
                cell.Environment.Cast().MagicEnergyLevel -= manaToRegenerate;
                Mana += manaToRegenerate;
            }
        }

        private void IncrementHunger()
        {
            hungerPercent = Math.Min(100d, hungerPercent + HungerIncrement);
            if (hungerPercent >= 100d)
            {
                Statuses.Add(new HungryObjectStatus());
            }
        }

        private void CheckOverweight()
        {
            var weight = Inventory.GetWeight();
            if (weight > MaxCarryWeight)
            {
                Statuses.Add(new OverweightObjectStatus());
            }
        }

        public override void OnDeath(Point position)
        {
            Log.Debug("Player is dead");

            base.OnDeath(position);

            Died?.Invoke(this, EventArgs.Empty);
        }

        public override ObjectSize Size => ObjectSize.Medium;

        public override int GetProtection(Element element)
        {
            var value = base.GetProtection(element) + Equipment.GetProtection(element);
            return Math.Min(MaxProtection, value);
        }

        public ILightSource[] LightSources => Equipment.GetLightSources();

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var body = GetBodyImage(storage);

            var equipmentImage = GetEquipmentImage(storage, body.Width, body.Height);
            body = SymbolsImage.Combine(body, equipmentImage);

            var directionImage = GetDirectionImage(storage);

            return SymbolsImage.Combine(body, directionImage);
        }

        private SymbolsImage GetDirectionImage(IImagesStorage storage)
        {
            var facingPosition = Point.GetPointInDirection(CurrentGame.PlayerPosition, Direction);
            var facingUsable = CurrentGame.Map.TryGetCell(facingPosition)?.Objects.OfType<IUsableObject>().Any() ?? false;

            switch (Direction)
            {
                case Direction.North:
                    return storage.GetImage(facingUsable ? ImageUpUsable : ImageUp);
                case Direction.South:
                    return storage.GetImage(facingUsable ? ImageDownUsable : ImageDown);
                case Direction.West:
                    return storage.GetImage(facingUsable ? ImageLeftUsable : ImageLeft);
                case Direction.East:
                    return storage.GetImage(facingUsable ? ImageRightUsable : ImageRight);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SymbolsImage GetEquipmentImage(IImagesStorage storage, int width, int height)
        {
            var result = new SymbolsImage(width, height);

            var equippedImages = Equipment.GetEquippedItems()
                .OfType<IEquippedImageProvider>()
                .OrderBy(item => item.EquippedImageOrder)
                .Select(item => item.GetEquippedImage(this, storage))
                .Where(image => image != null);

            foreach (var image in equippedImages)
            {
                result = SymbolsImage.Combine(result, image);
            }

            return result;
        }

        private SymbolsImage GetBodyImage(IImagesStorage storage)
        {
            var body = storage.GetImage(ImageBody);

            if (Equipment.RightWeaponEquipped)
            {
                var rightHandImage = storage.GetImage(ImageBodyRightWeapon);
                body = SymbolsImage.Combine(body, rightHandImage);
            }

            if (Equipment.LeftWeaponEquipped)
            {
                var leftHandImage = storage.GetImage(ImageBodyLeftWeapon);
                body = SymbolsImage.Combine(body, leftHandImage);
            }

            return body;
        }
    }
}