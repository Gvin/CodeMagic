using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items;
using CodeMagic.Game.Items.Materials;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class OreCaveWall : MinableWall
    {
        private const string ImageNormal = "Wall_Ore";
        private const string ImageBottom = "Wall_Ore_Bottom";
        private const string ImageRight = "Wall_Ore_Right";
        private const string ImageBottomRight = "Wall_Ore_Bottom_Right";
        private const string ImageCorner = "Wall_Ore_Corner";

        private readonly MetalType metalType;

        public OreCaveWall(MetalType metalType) 
            : base("Ore", 100)
        {
            this.metalType = metalType;
        }

        public override bool CanConnectTo(IMapObject mapObject)
        {
            return mapObject is CaveWall || mapObject is MinableWall;
        }

        protected override IItem CreateResource()
        {
            if (RandomHelper.CheckChance(50))
                return new Ore(metalType);

            return new Stone();
        }

        public override SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            var templateName = GetImageTemplate();
            var template = storage.GetImage(templateName);
            return MetalRecolorHelper.RecolorOreImage(template, metalType);
        }

        private string GetImageTemplate()
        {
            if (!HasConnectedTile(0, 1) && !HasConnectedTile(1, 0))
            {
                return ImageBottomRight;
            }

            if (!HasConnectedTile(0, 1))
            {
                return ImageBottom;
            }

            if (!HasConnectedTile(1, 0))
            {
                return ImageRight;
            }

            if (!HasConnectedTile(1, 1))
            {
                return ImageCorner;
            }

            return ImageNormal;
        }
    }
}