using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Extension
{
    public static class KeysHelper
    {
        public static (bool hasSymbol, char symbol) GetPressedChar(Keys key)
        {
            var str = key.ToString();
            if (str.Length == 1)
            {
                return (true, str.ToLower()[0]);
            }

            if (str.Length == 2 && str.StartsWith("D"))
            {
                return (true, str.Replace("D", "")[0]);
            }

            return (false, '\0');
        }
    }
}