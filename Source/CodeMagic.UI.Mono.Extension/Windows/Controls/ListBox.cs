using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.UI.Mono.Extension.Cells;
using Microsoft.Xna.Framework;

namespace CodeMagic.UI.Mono.Extension.Windows.Controls
{
    public class ListBox<TItem> : IControl where TItem : class, IListBoxItem
    {
        public event EventHandler SelectionChanged;

        private readonly VerticalScrollBar scroll;
        private readonly List<ItemWrapper> items;

        public ListBox(Rectangle location, VerticalScrollBar scroll)
        {
            Location = location;
            this.scroll = scroll;
            items = new List<ItemWrapper>();

            Enabled = true;
            Visible = true;

            UpdateScrollValue();
        }

        public Rectangle Location { get; set; }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

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
                if (value >= 0 && value < items.Count)
                {
                    items[value].Selected = true;
                }
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

        public void Draw(ICellSurface surface)
        {
            DrawItems(surface);
        }

        public void Update(TimeSpan elapsedTime)
        {
            UpdateScrollValue();
        }

        private void UpdateScrollValue()
        {
            scroll.MaxValue = Math.Max(0, items.Count - Location.Height);
        }

        public bool ProcessMouse(IMouseState mouseState)
        {
            if (!Location.Contains(mouseState.Position))
                return false;

            var scrollValue = mouseState.ScrollChange;
            if (scrollValue > 0)
            {
                scroll.Value++;
            }

            if (scrollValue < 0)
            {
                scroll.Value--;
            }

            if (mouseState.LeftButtonState)
            {
                var relativeY = mouseState.Position.Y - Location.Y;
                var index = relativeY + scroll.Value;
                if (index < 0 || index > items.Count - 1)
                    return true;

                SelectedItemIndex = index;
            }

            return true;
        }

        private void ClearSelection()
        {
            foreach (var wrapper in items)
            {
                wrapper.Selected = false;
            }
        }

        private void DrawItems(ICellSurface surface)
        {
            for (int index = 0; index < items.Count; index++)
            {
                var posY = index - scroll.Value;
                if (posY < 0 || posY >= Location.Height)
                    continue;

                var item = items[index];
                item.Draw(surface, Location.Width, posY);
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

            public void Draw(ICellSurface surface, int controlWidth, int y)
            {
                Item.Draw(surface, y, controlWidth, Selected);
            }
        }
    }

    public interface IListBoxItem
    {
        bool Equals(IListBoxItem other);

        void Draw(ICellSurface surface, int y, int maxWidth, bool selected);
    }
}