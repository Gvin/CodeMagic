using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Sad.Views;

namespace CodeMagic.UI.Sad.GameProcess
{
    public class DialogsProvider : IDialogsProvider
    {
        public void OpenInventoryDialog(string inventoryName, Inventory inventory)
        {
            new CustomInventoryView((GameCore<Player>) CurrentGame.Game, inventoryName, inventory).Show();
        }

        public void OpenWaitDialog(string message, Action waitAction)
        {
            new WaitMessageView(message, waitAction).Show();
        }
    }
}