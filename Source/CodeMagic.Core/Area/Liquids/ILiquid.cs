using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area.Liquids
{
    public interface ILiquid
    {
        int Volume { get; set; }

        int MaxVolume { get; }

        int MaxSpreadVolume { get; }

        ILiquid Separate(int volume);

        void Update(AreaMapCell cell);

        void ApplyEffect(IDestroyableObject destroyable);
    }
}