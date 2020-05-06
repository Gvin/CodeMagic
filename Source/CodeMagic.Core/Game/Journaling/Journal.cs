using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling
{
    public class Journal : IJournal
    {
        private readonly List<JournalMessageData> messages;

        public Journal()
        {
            messages = new List<JournalMessageData>();
        }

        public void Write(IJournalMessage message)
        {
            lock (messages)
            {
                messages.Add(new JournalMessageData(message, CurrentGame.Game.CurrentTurn));
            }
        }

        public void Write(IJournalMessage message, IMapObject source)
        {
            lock (messages)
            {
                if (IsObjectVisible(source))
                {
                    messages.Add(new JournalMessageData(message, CurrentGame.Game.CurrentTurn));
                }
            }
        }

        private static bool IsObjectVisible(IMapObject source)
        {
            var visibleArea = CurrentGame.Game.GetVisibleArea();
            for (int x = 0; x < visibleArea.Width; x++)
            {
                for (int y = 0; y < visibleArea.Height; y++)
                {
                    if (visibleArea.GetCell(x, y)?.Objects.Any(obj => ReferenceEquals(obj, source)) ?? false)
                        return true;
                }
            }

            return false;
        }

        public JournalMessageData[] Messages
        {
            get
            {
                lock (messages)
                {
                    return messages.ToArray();
                }
            }
        }
    }

    public class JournalMessageData
    {
        public JournalMessageData(IJournalMessage message, int turn)
        {
            Message = message;
            Turn = turn;
        }

        public IJournalMessage Message { get; }

        public int Turn { get; }
    }
}