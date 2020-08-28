using System;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public abstract class Control : IControl
    {
        protected Control(Rectangle location)
        {
            Location = location;
            Enabled = true;
            Visible = true;
        }

        public Rectangle Location { get; set; }

        public bool Enabled { get; set; }
        public bool Visible { get; set; }

        public virtual bool CanFocus => false;

        public bool Focused { get; set; }

        public abstract void Draw(ICellSurface surface);

        public virtual void Update(TimeSpan elapsedTime)
        {
            // Do nothing
        }

        public virtual void ProcessKeysPressed(Keys[] keys)
        {
            // Do nothing
        }

        public virtual void ProcessTextInput(char symbol)
        {
            // Do nothing
        }

        public virtual bool ProcessMouse(IMouseState mouseState)
        {
            return false;
        }
    }
}