using System;
using System.Collections.Generic;
using CodeMagic.UI.Console.Controls;

namespace CodeMagic.UI.Console.Views
{
    public abstract class View : IView
    {
        public event EventHandler Closed;
        public event EventHandler Closing;

        protected View()
        {
            Controls = new List<IConsoleControl>();
        }

        public List<IConsoleControl> Controls { get; }

        public virtual void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            foreach (var control in Controls)
            {
                if (control.ProcessKeyEvent(keyInfo))
                    break;
            }
        }

        public virtual void DrawStatic()
        {
            foreach (var control in Controls)
            {
                control.DrawStatic();
            }
        }

        public virtual void DrawDynamic()
        {
            foreach (var control in Controls)
            {
                control.DrawDynamic();
            }
        }

        public void Show()
        {
            ViewsManager.Current.AddView(this);
            OnShown();
        }

        public void Close()
        {
            Closing?.Invoke(this, EventArgs.Empty);

            ViewsManager.Current.RemoveView(this);
            OnClosed();

            Closed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnShown()
        {
        }

        protected virtual void OnClosed()
        {
        }
    }
}