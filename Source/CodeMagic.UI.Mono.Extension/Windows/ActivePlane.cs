using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.UI.Mono.Extension.Cells;
using CodeMagic.UI.Mono.Extension.Fonts;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Extension.Windows
{
    public class ActivePlane : IActivePlane
    {
        public ActivePlane(Point position, int width, int height, IFont font)
        {
            Position = new Point(position.X, position.Y);
            Width = width;
            Height = height;
            Font = font;

            Enabled = true;
            Visible = true;

            Controls = new List<IControl>();
        }

        protected List<IControl> Controls { get; private set; }

        public Point Position { get; set; }

        public int PixelWidth => Width * Font.GlyphWidth;

        public int PixelHeight => Height * Font.GlyphHeight;

        public int Width { get; }

        public int Height { get; }

        public IFont Font { get; }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        public virtual void Draw(ICellSurface surface)
        {
            foreach (var control in Controls.Where(c => c.Visible))
            {
                var subSurface = surface.GetSubSurface(control.Location);
                control.Draw(subSurface);
            }
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            foreach (var control in Controls.Where(c => c.Enabled))
            {
                control.Update(elapsedTime);
            }
        }

        public virtual bool ProcessMouse(IMouseState mouseState)
        {
            foreach (var control in Controls.Where(c => c.Enabled && c.Visible))
            {
                if (control.ProcessMouse(mouseState))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool ProcessKeyPressed(Keys key)
        {
            return false;
        }
    }
}