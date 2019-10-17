using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Materials;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class Tree : DestroyableObject, IWorldImageProvider, IResourceObject
    {
        private const int MaxWoodCount = 5;
        private const int MinWoodCount = 0;

        private const int LumberjackAxeDamageMultiplier = 2;

        private const string WorldImageName = "Tree";

        public Tree() : base(50)
        {
            BaseProtection.Add(Element.Electricity, 100);
            BaseProtection.Add(Element.Frost, 100);
            BaseProtection.Add(Element.Magic, 100);
            BaseProtection.Add(Element.Piercing, 50);
            BaseProtection.Add(Element.Blunt, 30);
            BaseProtection.Add(Element.Fire, -50);
        }

        public override string Name => "Tree";

        protected sealed override double CatchFireChanceMultiplier => 3;

        protected sealed override double SelfExtinguishChance => 5;

        public sealed override ObjectSize Size => ObjectSize.Huge;

        public override ZIndex ZIndex => ZIndex.BigDecoration;

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