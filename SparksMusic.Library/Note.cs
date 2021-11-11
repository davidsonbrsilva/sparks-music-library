using System;
using SparksMusic.Library.Enum;
using SparksMusic.Library.Extensions;

namespace SparksMusic.Library
{
    /// <summary>
    /// Note class
    /// </summary>
    public class Note : IEquatable<Note>
    {
        /// <summary>
        /// Note letter
        /// </summary>
        public NoteLetter NoteLetter { get; }

        /// <summary>
        /// Note accident
        /// </summary>
        public Accident Accident { get; }

        /// <summary>
        /// Creates a note object.
        /// </summary>
        /// <param name="noteLetter">A letra da nota (A, B, C, D, E, F ou G)</param>
        /// <param name="accident">O acidente da nota</param>
        public Note(NoteLetter noteLetter, Accident accident)
        {
            NoteLetter = noteLetter;
            Accident = accident;
        }

        /// <summary>
        /// Compare two notes.
        /// </summary>
        /// <param name="other">The other note</param>
        /// <returns>True if both have the same name.</returns>
        public bool Equals(Note other)
        {
            return other != null && ToString().Equals(other.ToString());
        }

        public override string ToString()
        {
            return $"{NoteLetter}{Accident.GetDescription()}";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as Note);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NoteLetter, Accident);
        }
    }
}
