using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.UI.Sad.Views
{
    public class ViewsManager
    {
        public static ViewsManager Current { get; } = new ViewsManager();

        private readonly List<View> views;

        private ViewsManager()
        {
            views = new List<View>();
        }

        public void AddView(View view)
        {
            views.Add(view);
            view.IsFocused = true;
        }

        public void RemoveView(View view)
        {
            views.Remove(view);

            if (CurrentView != null)
            {
                CurrentView.IsFocused = true;
            }
        }

        public void RemoveLastView()
        {
            if (views.Count > 0)
            {
                views.Remove(views.Last());
            }
            if (CurrentView != null)
            {
                CurrentView.IsFocused = true;
            }
        }

        public View CurrentView => views.LastOrDefault();
    }
}