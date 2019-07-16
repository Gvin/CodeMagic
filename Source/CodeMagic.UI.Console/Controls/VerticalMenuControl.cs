using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeMagic.UI.Console.Drawing.Writing;

namespace CodeMagic.UI.Console.Controls
{
    public class VerticalMenuControl<TData> : ConsoleControl
    {
        private int selectedItemIndex;

        public event EventHandler SelectionChanged; 

        public VerticalMenuControl()
        {
            Items = new List<MenuItem<TData>>();
            selectedItemIndex = -1;
            SelectedItem = null;
        }

        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
                SelectedItem = Items[selectedItemIndex];
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public MenuItem<TData> SelectedItem { get; private set; }

        public List<MenuItem<TData>> Items { get; }

        public Color SelectedFrameColor { get; set; }

        public Color TextColor { get; set; }

        public int? ItemWidth { get; set; }

        protected override void DrawDynamic(IControlWriter writer)
        {
            base.DrawDynamic(writer);

            var maxLength = ItemWidth ?? Items.Select(item => item.Text.Length).Max();

            var shiftY = 0;
            foreach (var menuItem in Items)
            {
                DrawItem(writer, menuItem, shiftY, maxLength);
                shiftY += 3;
            }
        }

        private void DrawItem(IControlWriter writer, MenuItem<TData> item, int shiftY, int maxLength)
        {
            if (SelectedItem == item)
            {
                writer.DrawFrame(0, shiftY, maxLength + 2, 3, false, SelectedFrameColor, Color.Black);
            }
            else
            {
                writer.ClearArea(0, shiftY, maxLength + 2, 3, Color.Black);
            }

            item.WriteText(1, shiftY + 1, maxLength, TextColor, writer);
        }

        

        protected override bool ProcessKey(ConsoleKeyInfo keyInfo)
        {
            var newSelectedItemIndex = SelectedItemIndex;
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    newSelectedItemIndex--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    newSelectedItemIndex++;
                    break;
                default:
                    return false;
            }

            if (newSelectedItemIndex < 0)
            {
                newSelectedItemIndex = Items.Count - 1;
            }

            if (newSelectedItemIndex > Items.Count - 1)
            {
                newSelectedItemIndex = 0;
            }

            SelectedItemIndex = newSelectedItemIndex;

            return true;
        }
    }

    public class MenuItem<T>
    {
        public MenuItem(string text, T data)
        {
            Text = text;
            Data = data;
        }

        public T Data { get; set; }

        public string Text { get; set; }

        public virtual void WriteText(int x, int y, int maxLength, Color textColor, IControlWriter writer)
        {
            writer.WriteAt(x, y, CutText(Text, maxLength), textColor, Color.Black);
        }

        protected string CutText(string initialText, int maxLength)
        {
            if (initialText.Length <= maxLength)
                return initialText;

            return initialText.Substring(0, maxLength);
        }
    }
}