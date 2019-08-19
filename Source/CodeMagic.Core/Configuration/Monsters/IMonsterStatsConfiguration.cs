namespace CodeMagic.Core.Configuration.Monsters
{
    public interface IMonsterStatsConfiguration
    {
        int MaxHealth { get; }

        int MinHealth { get; }

        float Speed { get; }

        int CatchFireChanceMultiplier { get; }

        int SelfExtinguishChanceMultiplier { get; }

        int VisibilityRange { get; }

        int HitChance { get; }

        IMonsterProtectionConfiguration[] Protection { get; }

        IMonsterDamageConfiguration[] Damage { get; }
    }
}