using System;
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
        private Keys[] oldPushedKeys;
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch spriteBatch;
        private int lastMouseScrollWheelValue;

        public MonoConsoleGame(int width, int height)
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = width, 
                PreferredBackBufferHeight = height
            };
            graphicsDeviceManager.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            oldPushedKeys = new Keys[0];

            Window.TextInput += Window_TextInput;
        }

        private void Window_TextInput(object sender, TextInputEventArgs e)
        {
            if (e.Key == Keys.Back || e.Key == Keys.Delete)
                return;

            var topWindow = WindowsManager.Instance.Windows.LastOrDefault();
            topWindow?.ProcessTextInput(e.Character);
        }

        protected virtual Color DefaultForeColor => Color.White;

        protected virtual Color DefaultBackColor => Color.Black;

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

            UpdateKeyboardState();

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

        private void UpdateKeyboardState()
        {
            var keyboard = Keyboard.GetState();
            var pushedKeys = keyboard.GetPressedKeys();

            var pressedKeys = oldPushedKeys.Where(key => !pushedKeys.Contains(key)).ToArray();
            ProcessKeysPressed(pressedKeys);

            oldPushedKeys = pushedKeys;
        }

        private void ProcessKeysPressed(Keys[] keys)
        {
            var topWindow = WindowsManager.Instance.Windows.LastOrDefault();
            if (topWindow != null && topWindow.Enabled)
            {
                topWindow.ProcessKeysPressed(keys);
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
