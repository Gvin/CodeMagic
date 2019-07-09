namespace CodeMagic.UI.Console.Drawing.DrawingProcessors
{
    public interface IDrawingProcessorsFactory
    {
        IDrawingProcessor GetProcessor(object @object);
    }
}