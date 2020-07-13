using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.UI.Mono.Extension.Fonts;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows
{
    public class Window : ActivePlane, IWindow
    {
        public Window(Point position, int width, int height, IFont font)
            : base(position, width, height, font)
        {
            ActivePlanes = new List<IActivePlane>();
        }

        protected List<IActivePlane> ActivePlanes { get; private set; }

        public IActivePlane[] GetActivePlanes()
        {
            return ActivePlanes.ToArray();
        }
        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            foreach (var activePlane in ActivePlanes.Where(p => p.Enabled))
            {
                activePlane.Update(elapsedTime);
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
    }
}