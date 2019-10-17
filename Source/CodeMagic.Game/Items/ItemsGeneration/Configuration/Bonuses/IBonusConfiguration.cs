using System.Collections.Generic;

namespace CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses
{
    public interface IBonusConfiguration
    {
        string Type { get; }

        Dictionary<string, string> Values { get; }
    }
}