using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI
{
    public class DialogsProvider : IDialogsProvider
    {
        private readonly IApplicationController controller;

        public DialogsProvider(IApplicationController controller)
        {
            this.controller = controller;
        }

        public void OpenInventoryDialog(string inventoryName, Inventory inventory)
        {
            controller.CreatePresenter<CustomInventoryPresenter>().Run((GameCore<Player>)CurrentGame.Game, inventoryName, inventory);
        }

        public void OpenWaitDialog(string message, Action waitAction)
        {
            controller.CreatePresenter<WaitMessagePresenter>().Run(message, waitAction);
        }
    }
}