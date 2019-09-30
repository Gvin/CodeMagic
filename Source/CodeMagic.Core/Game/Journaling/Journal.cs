using System.Collections.Generic;

namespace CodeMagic.Core.Game.Journaling
{
    public class Journal : IJournal
    {
        private readonly ITurnProvider turnProvider;
        private readonly List<JournalMessageData> messages;

        public Journal(ITurnProvider turnProvider)
        {
            this.turnProvider = turnProvider;
            messages = new List<JournalMessageData>();
        }

        public void Write(IJournalMessage message)
        {
            lock (messages)
            {
                messages.Add(new JournalMessageData(message, turnProvider.CurrentTurn));
            }
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