using System;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public abstract class ButtonBase : Control
    {
        protected ButtonBase(Rectangle location)
            : base(location)
        {
            Location = location;
            Enabled = true;
            Visible = true;
            Text = "Button";
        }

        protected bool IsHoldingClick { get; private set; }

        public event EventHandler Click;

        public string Text { get; set; }

        public override bool ProcessMouse(IMouseState mouseState)
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