using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.UI.Mono.Extension.Fonts;
using CodeMagic.UI.Mono.Extension.Windows.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Extension.Windows
{
    public class Window : ActivePlane, IWindow
    {
        public Window(Point position, int width, int height, IFont font)
            : base(position, width, height, font)
        {
            ActivePlanes = new List<IActivePlane>();
        }

        private IControl focusedControl;

        protected List<IActivePlane> ActivePlanes { get; private set; }

        public IActivePlane[] GetActivePlanes()
        {
            return ActivePlanes.ToArray();
        }
        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            if (!(FocusedControl?.CanFocus ?? false))
                FocusedControl = null;

            foreach (var activePlane in ActivePlanes.Where(p => p.Enabled))
            {
                activePlane.Update(elapsedTime);
            }
        }

        public override bool ProcessMouse(IMouseState mouseState)
        {
            if (mouseState.LeftButtonState)
            {
                FocusedControl = Controls.FirstOrDefault(c => c.CanFocus && c.Location.Contains(mouseState.Position)) ??
                                 FocusedControl;
            }

            return base.ProcessMouse(mouseState);
        }

        private IControl FocusedControl
        {
            get => focusedControl;
            set
            {
                if (focusedControl != null)
                {
                    focusedControl.Focused = false;
                }

                focusedControl = value;

                if (focusedControl != null)
                {
                    focusedControl.Focused = true;
                }
            }
        }

        public void Show()
        {
            WindowsManager.Instance.AddWindow(this);
        }

        public void Close()
        {
            WindowsManager.Instance.RemoveWindow(this);
        }

        public virtual bool ProcessKeysPressed(Keys[] keys)
        {
            if (focusedControl != null && focusedControl.Enabled)
            {
                focusedControl.ProcessKeysPressed(keys);
            }

            return false;
        }

        public virtual void ProcessTextInput(char symbol)
        {
            if (focusedControl != null && focusedControl.Enabled)
            {
                focusedControl.ProcessTextInput(symbol);
            }
        }
    }
}