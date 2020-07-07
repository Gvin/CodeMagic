using System;
using System.Linq;
using CodeMagic.UI.Services;

namespace CodeMagic.UI.Presenters
{
    public interface ISettingsView : IView
    {
        event EventHandler BrowseEditor;

        event EventHandler IncreaseFontSize;

        event EventHandler DecreaseFontSize;

        event EventHandler Exit;

        string FontSizeName { get; set; }

        string SpellEditorPath { get; set; }
    }

    public class SettingsPresenter : IPresenter
    {
        private readonly ISettingsView view;
        private readonly ISettingsService settings;

        public SettingsPresenter(ISettingsView view, ISettingsService settings)
        {
            this.view = view;
            this.settings = settings;

            this.view.SpellEditorPath = settings.SpellEditorPath;
            this.view.FontSizeName = GetFontSizeName(settings.FontSize);

            this.view.BrowseEditor += View_BrowseEditor;
            this.view.Exit += View_Exit;
            this.view.IncreaseFontSize += View_IncreaseFontSize;
            this.view.DecreaseFontSize += View_DecreaseFontSize;
        }

        private void View_DecreaseFontSize(object sender, EventArgs e)
        {
            SwitchFontSize(false);
            view.FontSizeName = GetFontSizeName(settings.FontSize);
        }

        private void View_IncreaseFontSize(object sender, EventArgs e)
        {
            SwitchFontSize(true);
            view.FontSizeName = GetFontSizeName(settings.FontSize);
        }

        private void View_Exit(object sender, EventArgs e)
        {
            view.Close();
        }

        private void View_BrowseEditor(object sender, EventArgs e)
        {
            throw new NotImplementedException("Not implemented");
        }

        public void Run()
        {
            view.Show();
        }

        private string GetFontSizeName(FontSizeMultiplier fontSize)
        {
            switch (fontSize)
            {
                case FontSizeMultiplier.X1:
                    return "x1";
                case FontSizeMultiplier.X2:
                    return "x2";
                default:
                    throw new ArgumentException($"Unknown font size: {fontSize}");
            }
        }

        private void SwitchFontSize(bool forward)
        {
            var diff = forward ? 1 : -1;
            var size = settings.FontSize;
            var sizes = Enum.GetValues(typeof(FontSizeMultiplier)).Cast<FontSizeMultiplier>().ToList();

            var currentIndex = sizes.IndexOf(size);
            var nextIndex = currentIndex + diff;
            nextIndex = Math.Max(0, nextIndex);
            nextIndex = Math.Min(sizes.Count - 1, nextIndex);

            settings.FontSize = sizes[nextIndex];
            settings.Save();
        }
    }
}