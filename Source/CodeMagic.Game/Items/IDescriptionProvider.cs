using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.Items
{
    public interface IDescriptionProvider
    {
        StyledLine[] GetDescription(Player player);
    }
}