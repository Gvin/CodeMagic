using System.Drawing;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Views
{
    public class EnterSpellDataView : View
    {
        private static readonly Color TextColor = Color.DarkGray;
        private static readonly Color ManaColor = Color.Blue;
        private static readonly Color NameColor = Color.White;

        public int? InitialManaLevel { get; set; }
        public int ManaLevel { get; private set; }
        public string Name { get; set; }

        public override void DrawStatic()
        {
            base.DrawStatic();

            Writer.CursorVisible = true;

            var result = false;
            while (!result)
            {
                ClearInputArea();
                result = GetData();
            }

            Writer.CursorVisible = false;

            Close();
        }

        private bool GetData()
        {
            Writer.CursorY = 3;
            if (!RequestManaCost())
                return false;

            Writer.CursorY++;

            return RequestName();
        }

        private bool RequestManaCost()
        {
            Writer.CursorX = 3;
            Writer.Write("Enter spell mana level", TextColor);
            if (InitialManaLevel.HasValue)
            {
                Writer.Write($" (was {InitialManaLevel.Value})", TextColor);
            }
            Writer.WriteLine(":", TextColor);

            Writer.CursorX = 3;
            Writer.ForeColor = ManaColor;
            var manaLevelString = Colorful.Console.ReadLine();
            if (string.IsNullOrEmpty(manaLevelString) && InitialManaLevel.HasValue)
            {
                ManaLevel = InitialManaLevel.Value;
            }
            else
            {
                if (!int.TryParse(manaLevelString, out var manaLevel))
                    return false;
                ManaLevel = manaLevel;
            }

            return true;
        }

        private bool RequestName()
        {
            Writer.CursorX = 3;
            Writer.Write("Enter spell name", TextColor);
            if (!string.IsNullOrEmpty(Name))
            {
                Writer.Write($" (was {Name})", TextColor);
            }
            Writer.WriteLine(":", TextColor);

            Writer.CursorX = 3;
            Writer.ForeColor = NameColor;
            var newName = Colorful.Console.ReadLine();
            if (string.IsNullOrEmpty(newName) && !string.IsNullOrEmpty(Name))
            {
                return true;
            }

            Name = newName;
            return true;
        }

        private void ClearInputArea()
        {
            Writer.BackColor = Color.Black;   
            for (var y = 3; y < 7; y++)
            {
                Writer.CursorY = y;
                Writer.CursorX = 1;
                for (var x = 1; x < Writer.ScreenWidth - 2; x++)
                {
                    Writer.Write(" ");
                }
            }
        }
    }
}