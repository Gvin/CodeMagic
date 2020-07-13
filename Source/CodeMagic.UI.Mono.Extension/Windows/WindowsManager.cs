using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.UI.Mono.Extension.Windows
{
    internal class WindowsManager
    {
        public static WindowsManager Instance { get; } = new WindowsManager();

        private readonly List<IWindow> windows;

        private WindowsManager()
        {
            windows = new List<IWindow>();
        }

        public void AddWindow(IWindow window)
        {
            if (windows.Contains(window))
                throw new ArgumentException("Such window was already added");

            windows.Add(window);
        }

        public void RemoveWindow(IWindow window)
        {
            if (!windows.Contains(window))
                throw new ArgumentException("Such window wasn't added");

            windows.Remove(window);
        }

        public IWindow[] Windows => windows.ToArray();
    }
}