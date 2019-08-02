using System;
using CodeMagic.Core.Items;
using SadConsole.Controls;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class ItemDetailsControl : ControlBase
    {
        public ItemDetailsControl(int width, int height) 
            : base(width, height)
        {
            Theme = new DrawingSurfaceTheme();
        }

        public InventoryStack Stack { get; set; }

        public override void Update(TimeSpan time)
        {
            base.Update(time);


        }
    }
}