using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    public class DungeonTrapDoor : MapObjectBase, IUsableObject, IWorldImageProvider
    {
        private const string ImageName = "Decoratives_TrapDoor";

        public DungeonTrapDoor(SaveData data) : base(data)
        {
        }

        public DungeonTrapDoor() : base("Trap Door")
        {
        }

        public override ZIndex ZIndex => ZIndex.BigDecoration;

        public SymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }

        public override ObjectSize Size => ObjectSize.Huge;

        public void Use(GameCore<Player> game, Point position)
        {
            DialogsManager.Provider.OpenWaitDialog("Descending...", () =>
            {
                var newMap = DungeonMapGenerator.Current.GenerateNewMap(game.Map.Level + 1, out var newPlayerPosition);
                game.ChangeMap(newMap, newPlayerPosition);
                game.Map.Refresh();
                game.Journal.Write(new DungeonLevelMessage(game.Map.Level));
            });
        }
    }
}