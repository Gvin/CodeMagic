using System;
using System.Drawing;
using Writer = Colorful.Console;

namespace CodeMagic.UI.Console.Views
{
    public class WaitForEditFinishView : View
    {
        public override void DrawStatic()
        {
            base.DrawStatic();

            Writer.CursorTop = 3;
            Writer.CursorLeft = 3;
            Writer.Write("Press ENTER to finish editing...", Color.White);
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            base.ProcessKey(keyInfo);

            if (keyInfo.Key == ConsoleKey.Enter)
                Close();
        }
    }
}