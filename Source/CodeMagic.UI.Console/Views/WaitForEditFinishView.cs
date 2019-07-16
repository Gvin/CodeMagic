using System;
using System.Drawing;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Views
{
    public class WaitForEditFinishView : View
    {
        public override void DrawStatic()
        {
            base.DrawStatic();

            Writer.CursorY = 3;
            Writer.CursorX = 3;
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