using System;
using System.Linq;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class JournalBoxControl : ControlBase
    {
        private const int MaxMessagesToDraw = 100;

        private static readonly Color FrameColor = Color.Gray;
        private static readonly Color BackColor = Color.Black;

        private readonly ScrollBar scroll;
        private readonly Journal journal;
        private readonly int maxMessagesCount;
        private readonly JournalMessageFormatter messageFormatter;

        public JournalBoxControl(int width, int height, ScrollBar scroll, Journal journal) 
            : base(width, height)
        {
            this.scroll = scroll;
            this.journal = journal;
            maxMessagesCount = height - 2;

            CanFocus = false;
            Theme = new DrawingSurfaceTheme();

            messageFormatter = new JournalMessageFormatter();
        }

        public override bool ProcessMouse(MouseConsoleState mouseState)
        {
            if (isMouseOver)
            {
                var scrollValue = mouseState.Mouse.ScrollWheelValueChange;
                if (scrollValue > 0)
                {
                    scroll.Value += scroll.Step;
                }

                if (scrollValue < 0)
                {
                    scroll.Value -= scroll.Step;
                }
            }

            return base.ProcessMouse(mouseState);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Surface.DrawBox(new Rectangle(0, 0, Width, Height), new Cell(Color.White, BackColor),
                new Cell(Color.White, BackColor));

            DrawFrame();

            var scrollMax = Math.Min(journal.Messages.Length, MaxMessagesToDraw) - maxMessagesCount;
            if (scrollMax < 0)
                scrollMax = 0;
            scroll.Maximum = scrollMax;

            DrawMessages();
        }

        private void DrawMessages()
        {
            DrawMessages(journal.Messages.Reverse().Take(MaxMessagesToDraw).ToArray());
        }

        private void DrawMessages(JournalMessageData[] messages)
        {
            const int dX = 3;
            var shiftY = 1 - scroll.Value;
            for (int index = 0; index < messages.Length; index++)
            {
                var yPos = shiftY + index;
                if (yPos < 1)
                    continue;

                var message = messages[index];
                var spreadMessage = SpreadMessage(message, Width - 2 - dX);
                foreach (var coloredString in spreadMessage)
                {
                    Surface.Print(dX, yPos, coloredString);
                    yPos++;
                }

                shiftY += spreadMessage.Length - 1;
            }
        }

        private ColoredString[] SpreadMessage(JournalMessageData message, int width)
        {
            var formattedMessage = messageFormatter.FormatMessage(message);
            return TextFormatHelper.SplitText(formattedMessage, width);
        }

        private void DrawFrame()
        {
            Surface.Fill(0, 0, Width, FrameColor, BackColor, Glyphs.GetGlyph('─'));
            Surface.Print(Width - 39, 0, new ColoredGlyph(Glyphs.GetGlyph('┴'), FrameColor, BackColor));
        }
    }
}