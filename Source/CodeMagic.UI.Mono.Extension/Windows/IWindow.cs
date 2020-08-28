using Microsoft.Xna.Framework.Input;

namespace CodeMagic.UI.Mono.Extension.Windows
{
    public interface IWindow : IActivePlane
    {
        IActivePlane[] GetActivePlanes();

        void Show();

        void Close();

        bool ProcessKeysPressed(Keys[] keys);

        void ProcessTextInput(char symbol);
    }
}