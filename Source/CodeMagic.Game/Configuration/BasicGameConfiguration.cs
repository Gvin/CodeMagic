using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeMagic.Game.Configuration
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class BasicGameConfiguration
    {
        public int SavingInterval { get; set; }
    }
}
