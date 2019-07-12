namespace CodeMagic.Core.Configuration
{
    public interface ISpellConfiguration
    {
        string SpellType { get; }

        double ManaCostMultiplier { get; }

        int ManaCostPower { get; }
    }
}