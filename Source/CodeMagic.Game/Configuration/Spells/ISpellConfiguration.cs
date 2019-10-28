namespace CodeMagic.Core.Configuration.Spells
{
    public interface ISpellConfiguration
    {
        string SpellType { get; }

        double ManaCostMultiplier { get; }

        int ManaCostPower { get; }

        ISpellConfigurationCustomValue[] CustomValues { get; }
    }

    public interface ISpellConfigurationCustomValue
    {
        string Key { get; }

        string Value { get; }
    }
}