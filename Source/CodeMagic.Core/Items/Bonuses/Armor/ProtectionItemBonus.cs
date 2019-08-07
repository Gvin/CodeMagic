using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Items.Bonuses.Armor
{
    public class ProtectionItemBonus : IArmorItemBonus
    {
        public ProtectionItemBonus(Element element, int protection)
        {
            Element = element;
            Protection = protection;
        }

        public Element Element { get; }

        public int Protection { get; }
    }
}