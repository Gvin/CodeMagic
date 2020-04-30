using CodeMagic.Core.Items;

namespace CodeMagic.Game
{
    public interface IDialogsProvider
    {
        void OpenInventoryDialog(string inventoryName, Inventory inventory);
    }

    public static class DialogsManager
    {
        public static IDialogsProvider Provider { get; private set; }

        public static void Initialize(IDialogsProvider provider)
        {
            Provider = provider;
        }
    }
}