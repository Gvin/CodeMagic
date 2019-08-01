using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects.PlayerData
{
    public class PlayerConfiguration : CreatureObjectConfiguration
    {
        public PlayerConfiguration()
        {
            BlindVisibilityRange = 0;
        }

        public int Mana { get; set; }

        public int MaxMana { get; set; }

        public int MaxWeight { get; set; }

        public int ManaRegeneration { get; set; }
    }
}