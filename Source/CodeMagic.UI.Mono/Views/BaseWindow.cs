using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows;
using CodeMagic.UI.Mono.Fonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Views
{
    public class BaseWindow : Window
    {
        private const int KeyPressedDelay = 100;
        private readonly Dictionary<Keys, TimeSpan> keysPressTime;

        protected static Color FrameColor = Color.Gray;

        public BaseWindow(FontTarget font) 
            : base(
                new Point(0, 0), 
                FontProvider.GetScreenWidthSymbols(font), 
                FontProvider.GetScreenHeightSymbols(font), 
                FontProvider.Instance.GetFont(font))
        {
            keysPressTime = new Dictionary<Keys, TimeSpan>();
        }

        public override void Draw(ICellSurface surface)
        {
            base.Draw(surface);

            surface.Fill(new Rectangle(1, 0, Width - 2, 1), new Cell('═', FrameColor));
            surface.Fill(new Rectangle(1, Height - 1, Width - 2, 1), new Cell('═', FrameColor));
            surface.Fill(new Rectangle(0, 1, 1, Height - 2), new Cell('║', FrameColor));
            surface.Fill(new Rectangle(Width - 1, 1, 1, Height - 2), new Cell('║', FrameColor));

            surface.SetCell(0, 0, '╔', FrameColor);
            surface.SetCell(Width - 1, 0, '╗', FrameColor);
            surface.SetCell(0, Height - 1, '╚', FrameColor);
            surface.SetCell(Width - 1, Height - 1, '╝', FrameColor);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            var keyboard = Keyboard.GetState();
            var keysPressed = keyboard.GetPressedKeys();

            foreach (var key in keysPressTime.Keys.ToArray())
            {
                if (!keysPressed.Contains(key))
                {
                    keysPressTime.Remove(key);
                }
            }

            foreach (var keyPressed in keysPressed)
            {
                if (keysPressTime.ContainsKey(keyPressed))
                {
                    keysPressTime[keyPressed] += elapsedTime;
                }
                else
                {
                    keysPressTime.Add(keyPressed, TimeSpan.Zero);
                }
            }

            foreach (var key in keysPressTime.Keys.ToArray())
            {
                if (keysPressTime[key] >= TimeSpan.FromMilliseconds(KeyPressedDelay))
                {
                    keysPressTime.Remove(key);
                    ProcessKeyPressed(key);
                }
            }
        }

        protected virtual void ProcessKeyPressed(Keys key)
        {
            // Do nothing
        }
    }
}