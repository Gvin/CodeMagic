using System;
using System.Linq;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Extension;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Controls
{
    public class JournalBoxControl : Control
    {
        private const int MaxMessagesToDraw = 100;

        private readonly Journal journal;
        private readonly VerticalScrollBar scroll;
        private readonly int maxMessagesCount;
        private readonly JournalMessageFormatter messageFormatter;

        public JournalBoxControl(Rectangle location, Journal journal, VerticalScrollBar scroll)
            : base(location)
        {
            this.journal = journal;
            
            this.scroll = scroll;
            messageFormatter = new JournalMessageFormatter();
            maxMessagesCount = Location.Height - 2;

            UpdateScrollMax();
        }

        public override void Draw(ICellSurface surface)
        {
           DrawMessages(surface, journal.Messages.Reverse().Take(MaxMessagesToDraw).ToArray());
        }

        private void DrawMessages(ICellSurface surface, JournalMessageData[] messages)
        {
            const int dX = 3;
            var shiftY = -scroll.Value;
            for (int index = 0; index < messages.Length; index++)
            {
                var yPos = shiftY + index;
                if (yPos < 0)
                    continue;

                var message = messages[index];
                var spreadMessage = SpreadMessage(message, Location.Width - 2 - dX);
                foreach (var coloredString in spreadMessage)
                {
                    if (surface.ContainsPoint(dX, yPos))
                    {
                        surface.Write(dX, yPos, coloredString);
                    }
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

        public override void Update(TimeSpan elapsedTime)
        {
            UpdateScrollMax();
        }

        private void UpdateScrollMax()
        {
            var scrollMax = Math.Min(journal.Messages.Length, MaxMessagesToDraw) - maxMessagesCount;
            if (scrollMax < 0)
                scrollMax = 0;
            scroll.MaxValue = scrollMax;
        }

        public override bool ProcessMouse(IMouseState mouseState)
        {
            if (!Location.Contains(mouseState.Position))
                return false;

            if (mouseState.ScrollChange > 0)
            {
                scroll.Value--;
            }

            if (mouseState.ScrollChange < 0)
            {
                scroll.Value++;
            }

            return true;
        }
    }
}