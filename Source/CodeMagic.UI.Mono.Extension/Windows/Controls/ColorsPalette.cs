using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class ColorsPalette
    {
        public ColorsPalette(Color? fore = null, Color? back = null)
        {
            Fore = fore;
            Back = back;
        }

        public Color? Fore { get; set; }

        public Color? Back { get; set; }

        public ColorsPalette Clone()
        {
            return new ColorsPalette
            {
                Fore = Fore,
                Back = Back
            };
        }
    }
}