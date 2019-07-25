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

            var scrollMax = journal.Messages.Length - maxMessagesCount;
            if (scrollMax < 0)
                scrollMax = 0;
            scroll.Maximum = scrollMax;

            DrawMessages();
        }

        private void DrawMessages()
        {
            DrawMessages(journal.Messages.Reverse().ToArray());
        }

        private void DrawMessages(JournalMessageData[] messages)
        {
            var shiftY = 1 - scroll.Value;
            for (int index = 0; index < messages.Length; index++)
            {
                var yPos = shiftY + index;
                if (yPos < 1)
                    continue;

                var message = messages[index];
                DrawJournalMessage(3, yPos, message);
            }
        }

        private void DrawJournalMessage(int x, int y, JournalMessageData message)
        {
            var formattedMessage = messageFormatter.FormatMessage(message);
            Surface.PrintStyledText(x, y, formattedMessage);
        }

        private void DrawFrame()
        {
            Surface.Fill(0, 0, Width, FrameColor, BackColor, Glyphs.GlyphBoxSingleHorizontal);
            Surface.Print(78, 0, new ColoredGlyph(Glyphs.GlyphBoxSingleHorizontalUp, FrameColor, BackColor));
        }
    }
}