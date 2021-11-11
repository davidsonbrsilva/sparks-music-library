using System;
using System.Diagnostics.CodeAnalysis;

namespace SparksMusic.Library
{
    [ExcludeFromCodeCoverage]
    public class NotAChordException : Exception
    {
        public NotAChordException() { }
        public NotAChordException(string message) : base(message) { }
        public NotAChordException(string message, Exception inner) : base(message, inner) { }
    }
    
}
