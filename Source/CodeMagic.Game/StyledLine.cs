using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CodeMagic.Game
{
    public class StyledLine : IEnumerable<StyledString>
    {
        private readonly List<StyledString> parts;

        public StyledLine()
        {
            parts = new List<StyledString>();
        }

        public void Add(StyledString part)
        {
            parts.Add(part);
        }

        public void Add(string text, Color? textColor = null)
        {
            parts.Add(new StyledString(text, textColor));
        }

        public void Add(IEnumerable<string> partsToAdd)
        {
            parts.AddRange(partsToAdd.Select(text => new StyledString(text)));
        }

        public void Add(IEnumerable<StyledString> partsToAdd)
        {
            parts.AddRange(partsToAdd);
        }

        public StyledString[] Parts => parts.ToArray();

        public IEnumerator<StyledString> GetEnumerator()
        {
            return parts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static StyledLine Empty => new StyledLine();
    }

    public class StyledString
    {
        public StyledString(object s, Color? textColor = null)
        {
            String = s.ToString();
            TextColor = textColor ?? Color.White;
        }

        public string String { get; set; }

        public Color TextColor { get; set; }
    }
}