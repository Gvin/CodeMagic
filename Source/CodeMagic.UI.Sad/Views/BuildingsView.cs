using System;
using System.Linq;
using CodeMagic.Configuration.Xml.Types.Buildings;
using CodeMagic.Core.Configuration;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Implementations.Objects.Buildings;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Controls;
using CodeMagic.UI.Sad.Views.BuildingUI;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Input;
using SadConsole.Themes;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Orientation = SadConsole.Orientation;
using Point = Microsoft.Xna.Framework.Point;
using ScrollBar = SadConsole.Controls.ScrollBar;

namespace CodeMagic.UI.Sad.Views
{
    public class BuildingsView : View
    {
        private readonly IGameCore game;

        private StandardButton closeButton;
        private CustomListBox<BuildingListBoxItem> buildingsListBox;
        private BuildingDetailsControl buildingDetails;

        private StandardButton placeSelectedBuildingButton;
        private StandardButton removeBuildingSiteButton;

        public BuildingsView(IGameCore game) 
            : base(Program.Width, Program.Height)
        {
            this.game = game;

            InitializeControls();
        }

        private void InitializeControls()
        {
            closeButton = new StandardButton(15)
            {
                Position = new Point(Width - 17, Height - 4),
                Text = "[ESC] Close"
            };
            closeButton.Click += closeButton_Click;
            Add(closeButton);

            placeSelectedBuildingButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, 16),
                Text = "[B] Build"
            };
            placeSelectedBuildingButton.Click += (sender, args) => PlaceSelectedBuilding();
            Add(placeSelectedBuildingButton);

            removeBuildingSiteButton = new StandardButton(25)
            {
                Position = new Point(Width - 57, Height - 4),
                Text = "[R] Remove Build Site"
            };
            removeBuildingSiteButton.Click += (sender, args) => RemoveBuildingSite();

            var playerLookPosition = Core.Game.Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);
            var playerLookCell = game.Map.GetCell(playerLookPosition);
            removeBuildingSiteButton.IsVisible = playerLookCell.Objects.OfType<BuildingSite>().Any();

            Add(removeBuildingSiteButton);

            buildingDetails = new BuildingDetailsControl(57, Height - 10)
            {
                Position = new Point(Width - 58, 3)
            };
            Add(buildingDetails);

            var scrollBarTheme = new ScrollBarTheme
            {
                Colors = new Colors
                {
                    Appearance_ControlNormal = new Cell(DefaultForeground, DefaultBackground)
                }
            };
            var scrollBar = new ScrollBar(Orientation.Vertical, Height - 4)
            {
                Position = new Point(Width - 60, 3),
                Theme = scrollBarTheme
            };
            Add(scrollBar);
            buildingsListBox = new CustomListBox<BuildingListBoxItem>(Width - 61, Height - 4, scrollBar)
            {
                Position = new Point(1, 3)
            };
            buildingsListBox.SelectionChanged += BuildingsListBoxSelectedItemChanged;
            Add(buildingsListBox);

            FillBuildings();

            UpdateBuildingDetails();
        }
        
        private void RemoveBuildingSite()
        {
            var playerLookPosition = Core.Game.Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);
            var playerTargetCell = game.Map.TryGetCell(playerLookPosition);

            var buildingSite = playerTargetCell?.Objects.OfType<BuildingSite>().FirstOrDefault();
            if (buildingSite == null)
                return;

            game.Map.RemoveObject(playerLookPosition, buildingSite);
            game.Journal.Write(new BuildingSiteRemovedMessage());
            Close();
        }

        private void PlaceSelectedBuilding()
        {
            var buildingConfiguration = buildingsListBox.SelectedItem?.Building;
            if (buildingConfiguration == null)
                return;

            var playerLookPosition = Core.Game.Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);
            var playerTargetCell = game.Map.TryGetCell(playerLookPosition);
            if (playerTargetCell == null || playerTargetCell.BlocksMovement)
            {
                game.Journal.Write(new CellBlockedForBuildingMessage());
                Close();
                return;
            }

            var building = CreateBuilding(buildingConfiguration.Type);
            game.Map.AddObject(playerLookPosition, new BuildingSite(buildingConfiguration, building));
            game.Journal.Write(new BuildingSitePlacesMessage(buildingConfiguration));
            Close();
        }

        private IMapObject CreateBuilding(Type buildingType)
        {
            if (!typeof(IMapObject).IsAssignableFrom(buildingType))
                throw new ApplicationException($"Invalid building type: {buildingType.FullName}");

            var constructor = buildingType.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new ApplicationException($"Unable to find constructor without arguments for building type: {buildingType.FullName}");

            var result = constructor.Invoke(new object[0]) as IMapObject;

            AddCustomBuildingHandlers(result);

            return result;
        }

        private void AddCustomBuildingHandlers(IMapObject building)
        {
            if (building is IStorageBuilding storageBuilding)
            {
                storageBuilding.Opened += (sender, args) => OpenStorage(storageBuilding);
            }

            if (building is Furnace furnace)
            {
                furnace.Used += (sender, args) => { new FurnaceUIView(game, furnace).Show(); };
            }
        }

        private void OpenStorage(IStorageBuilding storageBuilding)
        {
            new StorageInventoryView(game, storageBuilding.Name, storageBuilding.Inventory, storageBuilding.MaxWeight, null).Show();
        }

        private void FillBuildings()
        {
            buildingsListBox.ClearItems();

            var buildings = ConfigurationManager.Current.Buildings.Buildings
                .Where(game.Player.GetIfBuildingUnlocked)
                .Cast<XmlBuildingConfiguration>()
                .ToArray();
            foreach (var building in buildings)
            {
                buildingsListBox.AddItem(new BuildingListBoxItem(building));
            }

            buildingsListBox.SelectedItemIndex = 0;
        }

        private void BuildingsListBoxSelectedItemChanged(object sender, EventArgs args)
        {
            UpdateBuildingDetails();
        }

        private void UpdateBuildingDetails()
        {
            var selectedSpellItem = buildingsListBox.SelectedItem;
            buildingDetails.IsVisible = selectedSpellItem != null;
            buildingDetails.Building = selectedSpellItem?.Building;

            var buildingExists = selectedSpellItem?.Building != null;
            placeSelectedBuildingButton.IsVisible = buildingExists;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            Print(2, 1, "Available Buildings");

            Fill(1, 2, Width - 2, FrameColor, DefaultBackground, Glyphs.GetGlyph('─'));
            Print(0, 2, new ColoredGlyph(Glyphs.GetGlyph('╟'), FrameColor, DefaultBackground));
            Print(Width - 1, 2, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));

            Print(Width - 59, 2, new ColoredGlyph(Glyphs.GetGlyph('┬'), FrameColor, DefaultBackground));
            Print(Width - 59, Height - 1, new ColoredGlyph(Glyphs.GetGlyph('╧'), FrameColor, DefaultBackground));
            DrawVerticalLine(Width - 59, 3, Height - 4, new ColoredGlyph(Glyphs.GetGlyph('│'), FrameColor, DefaultBackground));

            if (buildingDetails.IsVisible)
            {
                Print(Width - 59, 4, new ColoredGlyph(Glyphs.GetGlyph('├'), FrameColor, DefaultBackground));
                Print(Width - 1, 4, new ColoredGlyph(Glyphs.GetGlyph('╢'), FrameColor, DefaultBackground));
            }
        }

        protected override bool ProcessKeyPressed(AsciiKey key)
        {
            switch (key.Key)
            {
                case Keys.Escape:
                    Close();
                    return true;
                case Keys.R:
                    RemoveBuildingSite();
                    return true;
                case Keys.B:
                    PlaceSelectedBuilding();
                    return true;
                case Keys.Up:
                case Keys.W:
                    MoveSelectionUp();
                    return true;
                case Keys.Down:
                case Keys.S:
                    MoveSelectionDown();
                    return true;
                default:
                    return false;
            }
        }

        private void MoveSelectionUp()
        {
            buildingsListBox.SelectedItemIndex = Math.Max(0, buildingsListBox.SelectedItemIndex - 1);
        }

        private void MoveSelectionDown()
        {
            buildingsListBox.SelectedItemIndex = Math.Min(buildingsListBox.Items.Length - 1, buildingsListBox.SelectedItemIndex + 1);
        }

        private void closeButton_Click(object sender, EventArgs args)
        {
            Close();
        }
    }

    public class BuildingListBoxItem : ICustomListBoxItem
    {
        private static readonly Color TextColor = Color.White;
        private static readonly Color SelectedItemBackColor = Color.FromNonPremultiplied(255, 128, 0, 255);
        private static readonly Color DefaultBackColor = Color.Black;

        public BuildingListBoxItem(XmlBuildingConfiguration building)
        {
            Building = building;
        }

        public bool Equals(ICustomListBoxItem other)
        {
            if (!(other is BuildingListBoxItem otherBuildingItem))
                return false;

            return otherBuildingItem.Building.Type == Building.Type;
        }

        public void Draw(CellSurface surface, int y, int maxWidth, bool selected)
        {
            var backColor = selected ? SelectedItemBackColor : DefaultBackColor;

            surface.Fill(0, y, maxWidth, null, backColor, null);
            surface.Print(2, y, Building.Name, new Cell(TextColor, backColor));
        }

        public XmlBuildingConfiguration Building { get; }
    }
}