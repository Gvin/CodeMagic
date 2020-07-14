using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

[assembly:InternalsVisibleTo("CodeMagic.UI.Mono.Extension.Tests")]


namespace CodeMagic.UI.Mono.Extension
{
    public class MonoConsoleGame : Game
    {
        private readonly Dictionary<Keys, TimeSpan> keysPressTime;
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private readonly int width;
        private readonly int height;
        private int lastMouseScrollWheelValue;

        public MonoConsoleGame(int width, int height)
        {
            this.width = width;
            this.height = height;

            graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = width, 
                PreferredBackBufferHeight = height
            };
            graphicsDeviceManager.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            keysPressTime = new Dictionary<Keys, TimeSpan>();
        }

        protected virtual Color DefaultForeColor => Color.White;

        protected virtual Color DefaultBackColor => Color.Black;

        protected virtual int KeyPressedDelay => 100;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var window in WindowsManager.Instance.Windows.Where(w => w.Enabled))
            {
                window.Update(gameTime.ElapsedGameTime);
            }

            UpdateKeyboardState(gameTime);

            var mouseState = Mouse.GetState();
            var topWindow = WindowsManager.Instance.Windows.LastOrDefault();
            if (topWindow != null)
            {
                ProcessMouseForActivePlane(topWindow, mouseState, 0, 0);

                foreach (var activePlane in topWindow.GetActivePlanes().Where(p => p.Visible && p.Enabled))
                {
                    ProcessMouseForActivePlane(activePlane, mouseState, topWindow.Position.X, topWindow.Position.Y);
                }

                lastMouseScrollWheelValue = mouseState.ScrollWheelValue;
            }

            base.Update(gameTime);
        }

        private void UpdateKeyboardState(GameTime gameTime)
        {
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
                    keysPressTime[keyPressed] += gameTime.ElapsedGameTime;
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

        private void ProcessKeyPressed(Keys key)
        {
            var topWindow = WindowsManager.Instance.Windows.LastOrDefault();
            if (topWindow != null && topWindow.Enabled)
            {
                var activePlanes = new List<IActivePlane>
                {
                    topWindow
                };
                activePlanes.AddRange(topWindow.GetActivePlanes());

                activePlanes = activePlanes.Where(p => p.Enabled).ToList();
                var processed = false;
                while (!processed && activePlanes.Count > 0)
                {
                    var plane = activePlanes[0];
                    processed = plane.ProcessKeyPressed(key);
                    activePlanes.Remove(plane);
                }
            }
        }

        private void ProcessMouseForActivePlane(IActivePlane plane, Microsoft.Xna.Framework.Input.MouseState mouseState, int shiftX, int shiftY)
        {
            if (!plane.Enabled || !plane.Visible)
                return;

            var planeRectangle = new Rectangle(plane.Position.X + shiftX, plane.Position.Y + shiftY, plane.PixelWidth, plane.PixelHeight);
            if (!planeRectangle.Contains(mouseState.Position))
                return;

            var mouseWindowX = (int)Math.Floor((double)(mouseState.X - plane.Position.X) / plane.Font.GlyphWidth);
            var mouseWindowY = (int)Math.Floor((double)(mouseState.Y - plane.Position.Y) / plane.Font.GlyphHeight);

            var scrollWheelDiff = mouseState.ScrollWheelValue - lastMouseScrollWheelValue;

            plane.ProcessMouse(new MouseState(
                new Point(mouseWindowX, mouseWindowY),
                mouseState.LeftButton == ButtonState.Pressed,
                mouseState.RightButton == ButtonState.Pressed,
                scrollWheelDiff));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(DefaultBackColor);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            DrawWindows();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawWindows()
        {
            var lastVisibleWindow = WindowsManager.Instance.Windows.LastOrDefault(window => window.Visible);
            if (lastVisibleWindow == null)
                return;

            DrawActivePlane(lastVisibleWindow, 0, 0);

            foreach (var activePlane in lastVisibleWindow.GetActivePlanes().Where(p => p.Visible))
            {
                DrawActivePlane(activePlane, lastVisibleWindow.Position.X, lastVisibleWindow.Position.Y);
            }
        }

        private void DrawActivePlane(IActivePlane plane, int shiftX, int shiftY)
        {
            var font = plane.Font;

            var surface = new CellSurface(plane.Width, plane.Height);
            plane.Draw(surface);

            for (int x = 0; x < surface.Width; x++)
            {
                for (int y = 0; y < surface.Height; y++)
                {
                    var cell = surface.GetCell(x, y);
                    var realX = x * font.GlyphWidth + plane.Position.X + shiftX;
                    var realY = y * font.GlyphHeight + plane.Position.Y + shiftY;
                    var drawPosition = new Vector2(realX, realY);
                    spriteBatch.Draw(
                        font.Texture,
                        drawPosition,
                        font.GetEmptyGlyphRect(),
                        cell.BackColor ?? DefaultBackColor);

                    if (cell.Glyph.HasValue)
                    {
                        var glyphRect = font.GetGlyphRect(cell.Glyph.Value);
                        spriteBatch.Draw(font.Texture, drawPosition, glyphRect, cell.ForeColor ?? DefaultForeColor);
                    }
                }
            }
        }
    }
}
