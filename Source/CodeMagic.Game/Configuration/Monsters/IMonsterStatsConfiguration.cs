namespace CodeMagic.Game.Configuration.Monsters
{
    public interface IMonsterStatsConfiguration
    {
        int MaxHealth { get; }

        int MinHealth { get; }

        float Speed { get; }

        int CatchFireChanceMultiplier { get; }

        int SelfExtinguishChanceMultiplier { get; }

        int VisibilityRange { get; }

        int Accuracy { get; }

        int DodgeChance { get; }

        IMonsterProtectionConfiguration[] Protection { get; }

        IMonsterDamageConfiguration[] Damage { get; }

        string[] StatusesImmunity { get; }
    }
}