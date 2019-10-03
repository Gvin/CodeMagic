using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Implementations.Items;
using CodeMagic.Implementations.Items.Materials;
using CodeMagic.UI.Images;

namespace CodeMagic.Implementations.Objects.SolidObjects
{
    public class Tree : DestroyableObject, IWorldImageProvider, IResourceObject
    {
        private const int MaxWoodCount = 5;
        private const int MinWoodCount = 0;

        private const int LumberjackAxeDamageMultiplier = 2;

        private const string WorldImageName = "Tree";

        public Tree() : base(new DestroyableObjectConfiguration
        {
            Name = "Tree",
            BaseProtection = new Dictionary<Element, int>
            {
                {Element.Electricity, 100},
                {Element.Frost, 100},
                {Element.Magic, 100},
                {Element.Piercing, 50},
                {Element.Blunt, 30},
                {Element.Fire, -50}
            },
            CatchFireChanceMultiplier = 3,
            SelfExtinguishChance = 5,
            Health = 50,
            MaxHealth = 50,
            Size = ObjectSize.Huge,
            ZIndex = ZIndex.BigDecoration
        })
        {
        }

        public override bool BlocksMovement => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }

        public void UseTool(IGameCore game, WeaponItem weapon, int damage, Element element)
        {
            if (weapon is LumberjackAxe axe)
            {
                var realDamage = damage * LumberjackAxeDamageMultiplier;
                if (RandomHelper.CheckChance(axe.LumberjackPower))
                {
                    game.Player.Inventory.AddItem(new Wood());
                }
                ApplyRealDamage(realDamage);
                return;
            }
            Damage(game.Journal, damage, element);
        }

        public override void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
            base.OnDeath(map, journal, position);

            var woodCount = RandomHelper.GetRandomValue(MinWoodCount, MaxWoodCount);
            for (var counter = 0; counter < woodCount; counter++)
            {
                map.AddObject(position, new Wood());
            }
        }
    }
}