using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Game.Journaling
{
    public class Journal
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
            messages.Add(new JournalMessageData(message, turnProvider.CurrentTurn));
        }

        public JournalMessageData[] GetLastMessages(int count)
        {
            return messages.Skip(Math.Max(0, messages.Count - count)).ToArray();
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