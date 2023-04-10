using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;
using CodeMagic.UI.Mono.Drawing;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows;
using CodeMagic.UI.Mono.Fonts;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;

namespace CodeMagic.UI.Mono.ActivePlanes
{
    public class GameAreaActivePlane : ActivePlane
    {
        private const int DurabilityIconPercent = 30;
        private const int DurabilityIconValue = 4;
        private const int PlayerFieldOfView = 4;

        private readonly GameCore<Player> _game;
        private readonly IImagesStorage _imagesStorage;
        private readonly ICellImageService _cellImageService;
        private AreaMapFragment _cachedVisibleArea;

        public GameAreaActivePlane(
            Point position,
            GameCore<Player> game,
            IImagesStorage imagesStorage,
            ICellImageService cellImageService)
            : base(position, 
                Program.MapCellImageSize * (PlayerFieldOfView * 2 + 1), 
                Program.MapCellImageSize * (PlayerFieldOfView * 2 + 1), 
                FontProvider.Instance.GetFont(FontTarget.Game))
        {
            _game = game;
            _imagesStorage = imagesStorage;
            _cellImageService = cellImageService;
            Position = position;
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            var currentVisibleArea = _game.GetVisibleArea() ?? _cachedVisibleArea;

            DrawMap(surface, currentVisibleArea);
            DrawDamagedIcons(surface);

            _cachedVisibleArea = currentVisibleArea;
        }

        private void DrawMap(ICellSurface surface, AreaMapFragment map)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var cell = map.GetCell(x, y);
                    DrawCell(surface, x, y, cell);
                }
            }
        }

        private void DrawCell(ICellSurface surface, int mapX, int mapY, IAreaMapCell cell)
        {
            var image = _cellImageService.GetCellImage(cell);

            var realX = mapX * Program.MapCellImageSize;
            var realY = mapY * Program.MapCellImageSize;

            surface.DrawImage(realX, realY, image, Color.White, Color.Black);
        }

        private void DrawDamagedIcons(ICellSurface surface)
        {
            var image = new SymbolsImage(3, 7);

            var images = _game.Player.Equipment.GetEquippedItems().OfType<DurableItem>().Where(ItemDamaged)
                .Select(GetDamagedIcon);
            foreach (var damagedImage in images)
            {
                image = SymbolsImage.Combine(image, damagedImage);
            }

            surface.DrawImage(Width - 4, Height - 8, image, Color.White, Color.Black);
        }

        private SymbolsImage GetDamagedIcon(DurableItem item)
        {
            var damageColor = TextHelper.GetDurabilityColor(item.Durability, item.MaxDurability);
            var iconName = GetDamagedIconName(item);
            var image = _imagesStorage.GetImage(iconName);
            return SymbolsImage.Recolor(image, new Dictionary<System.Drawing.Color, System.Drawing.Color>{
            {
                System.Drawing.Color.FromArgb(255, 0, 0),
                damageColor
            }});
        }

        private string GetDamagedIconName(DurableItem item)
        {
            if (item is ArmorItem armor)
            {
                switch (armor.ArmorType)
                {
                    case ArmorType.Helmet:
                        return "DamagedEquipment_Helmet";
                    case ArmorType.Chest:
                        return "DamagedEquipment_Chest";
                    case ArmorType.Leggings:
                        return "DamagedEquipment_Leggings";
                    default:
                        throw new ArgumentException($"Unknown armor type: {armor.ArmorType}");
                }
            }

            if (item is WeaponItem weapon)
            {
                if (_game.Player.Equipment.RightHandItem.Equals(weapon))
                {
                    return "DamagedEquipment_Weapon_Right";
                }
                else
                {
                    return "DamagedEquipment_Weapon_Left";
                }
            }

            throw new ApplicationException($"Unknown durable equipment: {item.GetType().Name}");
        }

        private bool ItemDamaged(DurableItem durable)
        {
            if (durable.Durability <= DurabilityIconValue)
                return true;

            var percent = (double)durable.Durability / durable.MaxDurability * 100;
            if (percent <= DurabilityIconPercent)
                return true;

            return false;
        }
    }
}