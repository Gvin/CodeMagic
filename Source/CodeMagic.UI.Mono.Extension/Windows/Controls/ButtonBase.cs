using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public abstract class ButtonBase : IControl
    {
        protected ButtonBase(Rectangle location)
        {
            Location = location;
            Enabled = true;
            Visible = true;
            Text = "Button";
        }

        protected bool IsHoldingClick { get; private set; }

        public event EventHandler Click;

        public string Text { get; set; }

        public Rectangle Location { get; set; }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        public abstract void Draw(ICellSurface surface);

        public void Update(TimeSpan elapsedTime)
        {
            // Do nothing
        }

        public virtual bool ProcessMouse(IMouseState mouseState)
        {
            if (Location.Contains(mouseState.Position))
            {
                if (mouseState.LeftButtonState)
                {
                    IsHoldingClick = true;
                }
                else if (IsHoldingClick)
                {
                    IsHoldingClick = false;
                    Click?.Invoke(this, EventArgs.Empty);
                }

                return true;
            }

            IsHoldingClick = false;
            return false;
        }
    }
}