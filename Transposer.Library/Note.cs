using System;
using Transposer.Library.Enum;
using Transposer.Library.Extensions;

namespace Transposer.Library
{
    /// <summary>
    /// Classe de nota.
    /// </summary>
    public class Note : IEquatable<Note>
    {
        public NoteLetter NoteLetter { get; }
        public Accident Accident { get; }

        /// <summary>
        /// Cria um objeto de nota.
        /// </summary>
        /// <param name="noteLetter">A letra da nota (A, B, C, D, E, F ou G)</param>
        /// <param name="accident">O acidente da nota</param>
        public Note(NoteLetter noteLetter, Accident accident)
        {
            NoteLetter = noteLetter;
            Accident = accident;
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

        public bool Equals(Note other)
        {
            return other != null && ToString().Equals(other.ToString());
        }
    }
}
