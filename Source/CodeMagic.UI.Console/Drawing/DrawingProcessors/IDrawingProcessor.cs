namespace CodeMagic.UI.Console.Drawing.DrawingProcessors
{
    public interface IDrawingProcessor
    {
        SymbolsImage GetImage(object @object);
    }
}