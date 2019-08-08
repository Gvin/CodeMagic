using System.Drawing;

namespace CodeMagic.Implementations.Items
{
    public interface IDescriptionProvider
    {
        StyledString[][] GetDescription();
    }

    public class StyledString
    {
        public StyledString(string s, Color? textColor = null)
        {
            String = s;
            TextColor = textColor ?? Color.White;
        }

        public string String { get; set; }

        public Color TextColor { get; set; }
    }
}