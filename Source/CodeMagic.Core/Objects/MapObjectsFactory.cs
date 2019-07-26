using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.DecorativeObjects;
using CodeMagic.Core.Objects.IceObjects;
using CodeMagic.Core.Objects.LiquidObjects;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Core.Objects.SteamObjects;
using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Objects
{
    public interface IMapObjectsCreator
    {
        ICodeSpell CreateCodeSpell(ICreatureObject caster, string name, string code, int mana);

        IFireDecorativeObject CreateFire(int temperature);

        IDecorativeObject CreateDecorativeObject(DecorativeObjectConfiguration configuration);

        TIce CreateIce<TIce>(int volume) where TIce : class, IIceObject;

        TLiquid CreateLiquid<TLiquid>(int volume) where TLiquid : class, ILiquidObject;

        TSteam CreateSteam<TSteam>(int volume) where TSteam : class, ISteamObject;

        IEnergyWall CreateEnergyWall(int lifeTime);
    }

    public static class MapObjectsFactory
    {
        private static IMapObjectsCreator creator;

        public static void Initialize(IMapObjectsCreator mapObjectsCreator)
        {
            creator = mapObjectsCreator;
        }

        public static ICodeSpell CreateCodeSpell(ICreatureObject caster, string name, string code, int mana)
        {
            return creator.CreateCodeSpell(caster, name, code, mana);
        }

        public static IFireDecorativeObject CreateFire(int temperature)
        {
            return creator.CreateFire(temperature);
        }

        public static IDecorativeObject CreateDecorativeObject(DecorativeObjectConfiguration configuration)
        {
            return creator.CreateDecorativeObject(configuration);
        }

        public static TIce CreateIceObject<TIce>(int volume) where TIce : class, IIceObject
        {
            return creator.CreateIce<TIce>(volume);
        }

        public static TLiquid CreateLiquidObject<TLiquid>(int volume) where TLiquid : class, ILiquidObject
        {
            return creator.CreateLiquid<TLiquid>(volume);
        }

        public static TSteam CreateSteam<TSteam>(int volume) where TSteam : class, ISteamObject
        {
            return creator.CreateSteam<TSteam>(volume);
        }

        public static IEnergyWall CreateEnergyWall(int lifeTime)
        {
            return creator.CreateEnergyWall(lifeTime);
        }
    }
}