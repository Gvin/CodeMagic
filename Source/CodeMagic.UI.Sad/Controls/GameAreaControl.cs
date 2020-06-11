using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;
using CodeMagic.UI.Sad.Common;
using CodeMagic.UI.Sad.Drawing;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class GameAreaControl : CustomControl
    {
        private static readonly Color DefaultForegroundColor = Color.White;
        private static readonly Color DefaultBackgroundColor = Color.Black;

        private const int ControlWidth = 93;
        private const int ControlHeight = 63;

        private const int DurabilityIconPercent = 30;
        private const int DurabilityIconValue = 4;

        private readonly GameCore<Player> game;

        static GameAreaControl()
        {
            Library.Default.SetControlTheme(typeof(GameAreaControl), new DrawingSurfaceTheme());
        }

        public GameAreaControl(GameCore<Player> game) 
            : base(ControlWidth, ControlHeight)
        {
            
            this.game = game;

            CanFocus = false;
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            var visibleArea = game.GetVisibleArea();

            if (visibleArea == null)
                return;

            for (int y = 0; y < visibleArea.Height; y++)
            {
                for (int x = 0; x < visibleArea.Width; x++)
                {
                    var cell = visibleArea.GetCell(x, y);
                    DrawCell(x, y, cell);
                }
            }

            DrawDamagedIcons();
        }

        private void DrawCell(int mapX, int mapY, IAreaMapCell cell)
        {
            var image = CellImageHelper.GetCellImage(cell);

            var realX = mapX * Program.MapCellImageSize;
            var realY = mapY * Program.MapCellImageSize;

            Surface.DrawImage(realX, realY, image, DefaultForegroundColor, DefaultBackgroundColor);

            DrawDebugData(realX, realY, cell);
        }

        private void DrawDamagedIcons()
        {
            var image = new SymbolsImage(3, 7);

            var images = game.Player.Equipment.GetEquippedItems().OfType<DurableItem>().Where(ItemDamaged)
                .Select(GetDamagedIcon);
            foreach (var damagedImage in images)
            {
                image = SymbolsImage.Combine(image, damagedImage);
            }

            Surface.DrawImage(Width - 4, Height - 8, image, DefaultForegroundColor, DefaultBackgroundColor);
        }

        private SymbolsImage GetDamagedIcon(DurableItem item)
        {
            var damageColor = TextHelper.GetDurabilityColor(item.Durability, item.MaxDurability);
            var iconName = GetDamagedIconName(item);
            var image = ImagesStorage.Current.GetImage(iconName);
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
                if (game.Player.Equipment.RightWeapon.Equals(weapon))
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

            var percent = (double) durable.Durability / durable.MaxDurability * 100;
            if (percent <= DurabilityIconPercent)
                return true;

            return false;
        }

        #region Debug Data Drawing

        private void DrawDebugData(int realX, int realY, IAreaMapCell cell)
        {
            if (Settings.Current.DebugDrawTemperature)
                DrawTemperature(realX, realY, cell);
            if (Settings.Current.DebugDrawLightLevel)
                DrawLightLevel(realX, realY + 1, cell);
            if (Settings.Current.DebugDrawMagicEnergy)
                DrawMagicEnergy(realX, realY + 1, cell);
        }

        private void DrawLightLevel(int x, int y, IAreaMapCell cell)
        {
            if (cell == null)
                return;

            var value = (int)cell.LightLevel;
            Surface.Print(x, y, new ColoredString(value.ToString(), Color.Yellow, Color.Black));
        }

        private void DrawTemperature(int x, int y, IAreaMapCell cell)
        {
            if (cell == null)
                return;

            var value = cell.Temperature() / 10;
            Surface.Print(x, y, new ColoredString(value.ToString(), Color.Red, Color.Black));
        }

        private void DrawMagicEnergy(int x, int y, IAreaMapCell cell)
        {
            if (cell == null)
                return;

            Surface.Print(x, y, new ColoredString(cell.MagicEnergyLevel().ToString(), Color.Blue, Color.Black));
            Surface.Print(x, y + 1, new ColoredString(cell.MagicDisturbanceLevel().ToString(), Color.Blue, Color.Black));
        }

        #endregion
    }
}