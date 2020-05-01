namespace CodeMagic.Game.Items.ItemsGeneration
{
    public static class ItemsGeneratorManager
    {
        public static IItemsGenerator Generator { get; private set; }

        public static void Initialize(IItemsGenerator generator)
        {
            Generator = generator;
        }
    }
}