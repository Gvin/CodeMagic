namespace CodeMagic.Core.Objects.PlayerData
{
    public class PlayerConfiguration : DestroyableObjectConfiguration
    {
        public int Mana { get; set; }

        public int MaxMana { get; set; }

        public int VisionRange { get; set; }

        public int MaxWeight { get; set; }

        public int ManaRegeneration { get; set; }
    }
}