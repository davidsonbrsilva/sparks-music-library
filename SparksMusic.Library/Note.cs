using SparksMusic.Library.Enum;
using SparksMusic.Library.Extensions;
using System;

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
        /// True if chord is flat
        /// </summary>
        public bool IsFlat { get => Accident == Accident.Flat; }

        /// <summary>
        /// True if chord is double flat
        /// </summary>
        public bool IsDoubleFlat { get => Accident == Accident.DoubleFlat; }

        /// <summary>
        /// True if chord is flat or double flat
        /// </summary>
        public bool IsFlatOrDoubleFlat { get => IsFlat || IsDoubleFlat; }

        /// <summary>
        /// True if chord is sharp
        /// </summary>
        public bool IsSharp { get => Accident == Accident.Sharp; }

        /// <summary>
        /// True if chord is double sharp
        /// </summary>
        public bool IsDoubleSharp { get => Accident == Accident.DoubleSharp; }

        /// <summary>
        /// True if chord is sharp or double sharp
        /// </summary>
        public bool IsSharpOrDoubleSharp { get => IsSharp || IsDoubleSharp; }

        /// <summary>
        /// Creates a note object.
        /// </summary>
        /// <param name="noteLetter">The note letter (A, B, C, D, E, F or G)</param>
        /// <param name="accident">The note accident</param>
        public Note(NoteLetter noteLetter, Accident accident = Accident.None)
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
