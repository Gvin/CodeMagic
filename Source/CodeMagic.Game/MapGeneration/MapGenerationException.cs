using System;

namespace CodeMagic.Game.MapGeneration
{
    public class MapGenerationException : Exception
    {
        private const string DefaultMessage = "Unable to generate map.";

        public MapGenerationException(string message) : base(message)
        {
        }

        public MapGenerationException()
            : base(DefaultMessage)
        {
        }

        public MapGenerationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}