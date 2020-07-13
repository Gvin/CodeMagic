using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Game.JournalMessages;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Drawing
{
    // public class JournalMessageFormatter
    // {
    //     private static readonly Color TurnsCountColor = Color.Gray;
    //     private static readonly Color BackgroundColor = Color.Black;
    //
    //     public ColoredString[] FormatMessage(JournalMessageData messageData)
    //     {
    //         var formattedMessage = new List<ColoredString>
    //         {
    //             new ColoredString($"[{messageData.Turn}] ", TurnsCountColor, BackgroundColor)
    //         };
    //         formattedMessage.AddRange(GetMessageBody(messageData.Message));
    //         return formattedMessage.ToArray();
    //     }
    //
    //     private ColoredString[] GetMessageBody(IJournalMessage message)
    //     {
    //         if (message is ISelfDescribingJournalMessage selfDescribingMessage)
    //         {
    //             return selfDescribingMessage.GetDescription().Parts.Select(styledString =>
    //                     new ColoredString(styledString.String.ConvertGlyphs(),
    //                         new Cell(styledString.TextColor.ToXna(), BackgroundColor)))
    //                 .ToArray();
    //         }
    //
    //         throw new ApplicationException("Journal message should be ISelfDescribingJournalMessage");
    //     }
    // }
}