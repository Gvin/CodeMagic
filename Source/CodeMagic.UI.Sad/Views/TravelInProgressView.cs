namespace CodeMagic.UI.Sad.Views
{
    public class TravelInProgressView : View
    {
        public TravelInProgressView() 
            : base(Program.Width, Program.Height)
        {

            Print(10, 10, "Traveling to another location...");
        }
    }
}