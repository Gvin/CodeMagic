namespace CodeMagic.UI.Mono.Extension.Windows
{
    public interface IWindow : IActivePlane
    {
        IActivePlane[] GetActivePlanes();

        void Show();

        void Close();
    }
}