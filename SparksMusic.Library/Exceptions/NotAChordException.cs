using System;

namespace SparksMusic.Library
{
    public class NotAChordException : Exception
    {
        public NotAChordException() { }
        public NotAChordException(string message) : base(message) { }
        public NotAChordException(string message, Exception inner) : base(message, inner) { }
    }

}
