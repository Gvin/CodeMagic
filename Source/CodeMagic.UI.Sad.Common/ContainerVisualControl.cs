using System.Collections.Generic;
using SadConsole;
using SadRogue.Primitives;

namespace CodeMagic.UI.Sad.Common
{
    public class ContainerVisualControl : IVisualControl
    {
        private readonly List<IVisualControl> controls;

        public ContainerVisualControl(int x, int y, int width, int height)
            : this(new Rectangle(x, y, width, height))
        {
        }

        public ContainerVisualControl(Rectangle position)
        {
            controls = new List<IVisualControl>();
            Position = position;
            IsVisible = true;
        }

        public Rectangle Position { get; }

        public bool IsVisible { get; set; }

        public void Add(IVisualControl control)
        {
            controls.Add(control);
        }

        public void Remove(IVisualControl control)
        {
            controls.Remove(control);
        }

        public virtual void Draw(ICellSurface surface)
        {
            surface.Clear(new Rectangle(0, 0, Position.Width, Position.Height));

            foreach (var control in controls)
            {
                if (!control.IsVisible)
                    continue;

                var controlSurface = surface.GetSubSurface(control.Position);
                control.Draw(controlSurface);
            }
        }
    }
}