using System;
using System.Collections.Generic;
using System.Linq;
using SadConsole;
using SadConsole.Controls;
using SadConsole.Input;
using SadConsole.Themes;

namespace CodeMagic.UI.Sad.Controls
{
    public class CustomListBox<TItem> : ControlBase where TItem : class, ICustomListBoxItem
    {
        public event EventHandler SelectionChanged;

        private readonly ScrollBar scroll;
        private readonly List<ItemWrapper> items;

        public CustomListBox(int width, int height, ScrollBar scroll) 
            : base(width, height)
        {
            Theme = new DrawingSurfaceTheme();
            CanFocus = false;

            this.scroll = scroll;
            items = new List<ItemWrapper>();
        }

        public TItem[] Items => items.Select(wrapper => wrapper.Item).ToArray();

        public TItem SelectedItem
        {
            get => items.FirstOrDefault(wrapper => wrapper.Selected)?.Item;
            set
            {
                var wrapper = items.FirstOrDefault(wr => wr.Item.Equals(value));
                if (wrapper == null)
                    throw new ApplicationException("Unable to find item to select in items.");
                ClearSelection();
                wrapper.Selected = true;
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                var selectedWrapper = items.FirstOrDefault(wrapper => wrapper.Selected);
                if (selectedWrapper == null)
                    return -1;

                return items.IndexOf(selectedWrapper);
            }
            set
            {
                ClearSelection();
                items[value].Selected = true;
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void AddItem(TItem item)
        {
            items.Add(new ItemWrapper(item));
        }

        public void ClearItems()
        {
            items.Clear();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void Update(TimeSpan time)
        {
            Surface.Clear();

            base.Update(time);

            scroll.Maximum = Math.Max(0, items.Count - Height);

            DrawItems();
        }

        public override bool ProcessMouse(MouseConsoleState mouseState)
        {
            if (isMouseOver && CustomProcessMouse(mouseState))
                return true;

            return base.ProcessMouse(mouseState);
        }

        private bool CustomProcessMouse(MouseConsoleState mouseState)
        {
            if (mouseState.Mouse.LeftClicked)
            {
                var relativeY = mouseState.ConsoleCellPosition.Y - Position.Y;
                var index = relativeY + scroll.Value;
                if (index < 0 || index > items.Count - 1)
                    return false;

                SelectedItemIndex = index;
                return true;
            }

            var scrollValue = mouseState.Mouse.ScrollWheelValueChange;
            if (scrollValue > 0)
            {
                scroll.Value += scroll.Step;
                return true;
            }

            if (scrollValue < 0)
            {
                scroll.Value -= scroll.Step;
                return true;
            }

            return false;
        }

        private void ClearSelection()
        {
            foreach (var wrapper in items)
            {
                wrapper.Selected = false;
            }
        }

        private void DrawItems()
        {
            for (int index = 0; index < items.Count; index++)
            {
                var posY = index - scroll.Value;
                if (posY < 0 || posY >= Height)
                    continue;

                var item = items[index];
                item.Draw(this, posY);
            }
        }

        private class ItemWrapper
        {
            public ItemWrapper(TItem item)
            {
                Item = item;
                Selected = false;
            }

            public TItem Item { get; }

            public bool Selected { get; set; }

            public void Draw(CustomListBox<TItem> parent, int y)
            {
                Item.Draw(parent.Surface, y, parent.Width, Selected);
            }
        }
    }

    public interface ICustomListBoxItem
    {
        bool Equals(ICustomListBoxItem other);

        void Draw(CellSurface surface, int y, int maxWidth, bool selected);
    }
}