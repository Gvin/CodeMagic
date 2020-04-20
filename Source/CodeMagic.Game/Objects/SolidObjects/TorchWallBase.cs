using CodeMagic.Core.Area;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public abstract class TorchWallBase : WallBase, ILightObject
    {
        protected TorchWallBase(SaveData data) : base(data)
        {
        }

        protected TorchWallBase(string name) : base(name)
        {
        }

        private const LightLevel DefaultLightLevel = LightLevel.Medium;

        private LightLevel LightPower => DefaultLightLevel;

        public ILightSource[] LightSources => new ILightSource[]
        {
            new StaticLightSource(LightPower)
        };
    }
}