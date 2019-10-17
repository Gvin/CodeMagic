using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Game.Items
{
    public interface IDescriptionProvider
    {
        StyledLine[] GetDescription(IPlayer player);
    }
}