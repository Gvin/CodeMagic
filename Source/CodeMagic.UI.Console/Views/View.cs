using System;

namespace CodeMagic.UI.Console.Views
{
    public abstract class View : IView
    {
        public event EventHandler Closed;
        public event EventHandler Closing;

        public virtual void DrawStatic()
        {
        }

        public virtual void DrawDynamic()
        {
        }

        public virtual void ProcessKey(ConsoleKeyInfo keyInfo)
        {
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