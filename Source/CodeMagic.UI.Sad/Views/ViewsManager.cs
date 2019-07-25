using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.UI.Sad.Views
{
    public class ViewsManager
    {
        public static ViewsManager Current { get; } = new ViewsManager();

        private readonly Stack<View> views;

        private ViewsManager()
        {
            views = new Stack<View>();
        }

        public void AddView(View view)
        {
            views.Push(view);
            view.IsFocused = true;
        }

        public void RemoveLastView()
        {
            views.Pop();
            if (CurrentView != null)
            {
                CurrentView.IsFocused = true;
            }
        }

        public View CurrentView => views.FirstOrDefault();
    }
}