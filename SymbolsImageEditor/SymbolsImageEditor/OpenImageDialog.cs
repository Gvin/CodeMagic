using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMagic.UI.Sad.Common;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace SymbolsImageEditor
{
    public class OpenImageDialog : View
    {
        private readonly ListBox list;
        private readonly Button okButton;
        private readonly Button cancelButton;

        private Action<DialogResult> callback;

        public OpenImageDialog() 
            : base(new NullLog(), Program.Width, Program.Height, Program.Font)
        {
            okButton = new Button(7)
            {
                Position = new Point(2, Height - 3),
                Text = "OK"
            };
            okButton.Click += (sender, args) => Close(DialogResult.Ok);
            Add(okButton);

            cancelButton = new Button(10)
            {
                Position = new Point(15, Height - 3),
                Text = "Cancel"
            };
            cancelButton.Click += (sender, args) => Close(DialogResult.Cancel);
            Add(cancelButton);

            list = new ListBox(Width, Height - 10)
            {
                Position = new Point(1, 4)
            };
            list.SelectedItemChanged += ListOnSelectedItemChanged;
            Add(list);

            Path = @"\";
            InitList();
        }

        public void ShowModal(Action<DialogResult> callbackFunction)
        {
            callback = callbackFunction;
            Show();
        }

        private void ListOnSelectedItemChanged(object sender, ListBox.SelectedItemEventArgs e)
        {
            if (e.Item == null)
                return;

            var subPath = e.Item.ToString();
            if (string.Equals(subPath, ".."))
            {
                Path = Path.Substring(0, Path.LastIndexOf(@"\"));
            }
            else
            {
                Path = System.IO.Path.Combine(Path, subPath);
            }

            InitList();

            okButton.IsEnabled = File.Exists(Path);
        }

        public string Path { get; set; }

        private void InitList()
        {
            list.Items.Clear();

            if (!string.Equals(Path, @"\"))
                list.Items.Add("..");
            foreach (var accessiblePath in GetAccessiblePaths())
            {
                list.Items.Add(accessiblePath);
            }
        }

        private string[] GetAccessiblePaths()
        {
            if (string.Equals(Path, @"\"))
                return GetDrives();

            var selectedPath = Path;
            if (!Directory.Exists(selectedPath))
            {
                selectedPath = System.IO.Path.GetDirectoryName(selectedPath);
            }
            var dirs = Directory.GetDirectories(selectedPath).Select(dir => dir.Substring(dir.LastIndexOf(@"\") + 1)).Select(dir => @$"{dir}\");
            var files = Directory.GetFiles(selectedPath, "*.simg", SearchOption.TopDirectoryOnly).Select(dir => dir.Substring(dir.LastIndexOf(@"\") + 1));
            var result = new List<string>();
            result.AddRange(dirs);
            result.AddRange(files);
            return result.ToArray();
        }

        private string[] GetDrives()
        {
            return DriveInfo.GetDrives().Select(drive => drive.Name).ToArray();
        }

        protected override void DrawView(CellSurface surface)
        {
            base.DrawView(surface);

            surface.Print(1, 1, "Open Image");
            surface.Clear(new Rectangle(1, 2, Width - 2, 2));
            surface.Print(1, 2, Path);
        }

        protected override void OnClosed(DialogResult result)
        {
            base.OnClosed(result);

            callback?.Invoke(result);
        }
    }
}