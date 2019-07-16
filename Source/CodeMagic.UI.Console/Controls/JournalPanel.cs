using System;
using System.Drawing;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.UI.Console.Drawing;
using CodeMagic.UI.Console.Drawing.JournalTextProviding;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Controls
{
    public class JournalPanel : ConsoleControl
    {
        private const int MaxJournalMessagesCount = 5;

        private readonly Color frameColor;
        private readonly Journal journal;
        private readonly JournalTextProviderFactory journalTextProviderFactory;
        private int journalScroll;

        public JournalPanel(Journal journal, Color frameColor)
        {
            this.journal = journal;
            this.frameColor = frameColor;

            journalTextProviderFactory = new JournalTextProviderFactory();

            journalScroll = 0;
        }

        public int JournalScroll
        {
            get => journalScroll;
            set
            {
                if (value < 0)
                {
                    journalScroll = 0;
                }
                else
                {
                    journalScroll = value;
                }
            }
        }

        protected override void DrawStatic(IControlWriter writer)
        {
            base.DrawStatic(writer);

            writer.Write(LineTypes.DoubleVerticalSingleRight, frameColor);
            writer.DrawHorizontalLine(0, 1, Width - 2, false, frameColor);
            writer.Write(LineTypes.DoubleVerticalSingleLeft, frameColor);
        }

        protected override void DrawDynamic(IControlWriter writer)
        {
            base.DrawDynamic(writer);

            writer.CursorY = 1;
            writer.BackColor = Color.Black;

            var messages = journal.GetMessages(JournalScroll + MaxJournalMessagesCount, MaxJournalMessagesCount);

            for (var index = 0; index < MaxJournalMessagesCount; index++)
            {
                var message = messages.Length > index ? messages[index] : null;
                if (message != null)
                {
                    var text = $"[{message.Turn}] " + journalTextProviderFactory.GetMessageText(message.Message);
                    text = text.Length > Width - 2 ? text.Substring(0, Width - 2) : text;
                    writer.CursorX = 2;
                    while (text.Length < Width - 3)
                        text += " ";
                    writer.WriteLine(text, Color.DarkGray);
                }
                else
                {
                    writer.CursorX = 2;
                    for (var x = writer.CursorX; x < Width - 2; x++)
                    {
                        writer.Write(" ");
                    }

                    writer.CursorY = writer.CursorY < Height - 1 ? writer.CursorY + 1 : 0;
                }
            }

            var messagesCount = journal.Messages.Length;
            if (JournalScroll > journal.Messages.Length - MaxJournalMessagesCount)
            {
                JournalScroll = messagesCount - MaxJournalMessagesCount;
            }

            writer.CursorY = 1;
            writer.CursorX = 1;
            var messagesToTheUp = messagesCount - JournalScroll;
            writer.Write('\u25B2', messagesToTheUp > MaxJournalMessagesCount ? Color.Yellow : frameColor);

            writer.CursorY = Height - 2;
            writer.CursorX = 1;
            writer.Write('\u25BC', JournalScroll > 0 ? Color.Yellow : frameColor);
        }

        protected override bool ProcessKey(ConsoleKeyInfo keyInfo)
        {
            base.ProcessKey(keyInfo);

            switch (keyInfo.Key)
            {
                case ConsoleKey.PageUp:
                    JournalScroll++;
                    break;
                case ConsoleKey.PageDown:
                    JournalScroll--;
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}