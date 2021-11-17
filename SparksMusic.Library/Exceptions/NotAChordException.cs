using System;

namespace SparksMusic.Library
{
    /// <summary>
    /// NotAChord exception
    /// </summary>
    public class NotAChordException : Exception
    {
        /// <summary>
        /// NotAChord exception constructor
        /// </summary>
        public NotAChordException() { }

        /// <summary>
        /// NotAChord exception constructor
        /// </summary>
        /// <param name="message">The custom message of the exception</param>
        public NotAChordException(string message) : base(message) { }

        /// <summary>
        /// NotAChord exception constructor
        /// </summary>
        /// <param name="message">The custom message of the exception</param>
        /// <param name="inner">The inner exception</param>
        public NotAChordException(string message, Exception inner) : base(message, inner) { }
    }

}
