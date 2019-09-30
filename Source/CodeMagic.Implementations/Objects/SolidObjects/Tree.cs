using System;
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
        private const int TotalWoodCount = 20;
        private const int LumberjackAxeDamageMultiplier = 2;

        private const string WorldImageName = "Tree";
        private bool isChop;
        private readonly List<HitData> hitData;

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
            hitData = new List<HitData>();
            isChop = false;
        }

        public override bool BlocksMovement => true;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(WorldImageName);
        }

        public void UseTool(WeaponItem weapon, IJournal journal, int damage, Element element)
        {
            isChop = true;

            var realDamage = GetToolDamage(weapon, damage);
            var toolPower = GetToolPower(weapon);
            hitData.Add(new HitData(realDamage, toolPower));

            Damage(journal, realDamage, element);
            
            isChop = false;
        }

        private int GetToolDamage(WeaponItem item, int damage)
        {
            if (item is LumberjackAxe)
                return damage * LumberjackAxeDamageMultiplier;

            return damage;
        }

        private int GetToolPower(WeaponItem item)
        {
            if (item is LumberjackAxe axe)
                return axe.LumberjackPower;

            return 10;
        }

        public override void Damage(IJournal journal, int damage, Element element)
        {
            base.Damage(journal, damage, element);

            if (!isChop)
            {
                hitData.Add(new HitData(damage, 0));
            }
        }

        public override void OnDeath(IAreaMap map, IJournal journal, Point position)
        {
            base.OnDeath(map, journal, position);

            var woodCount = GetWoodCount();
            for (var counter = 0; counter < woodCount; counter++)
            {
                map.AddObject(position, new Wood());
            }
        }

        private int GetWoodCount()
        {
            var result = 0d;
            foreach (var data in hitData)
            {
                var percent = data.Damage / (double) MaxHealth;
                result += percent * data.ToolPower;
            }

            return (int) Math.Round(TotalWoodCount * result / 100);
        }

        private class HitData
        {
            public HitData(int damage, int toolPower)
            {
                Damage = damage;
                ToolPower = toolPower;
            }

            public int Damage { get; }

            public int ToolPower { get; }
        }
    }
}