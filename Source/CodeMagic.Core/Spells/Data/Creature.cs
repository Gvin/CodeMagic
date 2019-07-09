namespace CodeMagic.Core.Spells.Data
{
    public class Creature
    {
        public const string PlayerId = "0";

        public Creature(dynamic creatureData)
        {
            Id = (string)creatureData.id;
        }

        public string Id { get; }
    }
}