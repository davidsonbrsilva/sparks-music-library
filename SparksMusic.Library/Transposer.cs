using System;
using System.Collections.Generic;
using System.Linq;

namespace SparksMusic.Library
{
    /// <summary>
    /// Transposer class
    /// </summary>
    public static class Transposer
    {
        private static readonly Node _flatMap = BuildFlatMap();
        private static readonly Node _sharpMap = BuildSharpMap();
        private static readonly Dictionary<Note, Note> _chromaticCorrespondentDictionary = BuildChromaticCorrespondentDictionary();

        private const int SemitonesOnTheScale = 12;

        #region Public Methods
        /// <summary>
        /// Transposes up a chord.
        /// </summary>
        /// <param name="chord">The chord</param>
        /// <param name="semitones">The semitones to the transposition</param>
        /// <returns>A transposed chord.</returns>
        /// <exception cref="ArgumentNullException">Thrown when chord object is null.</exception>
        /// <exception cref="NotAChordException">Thrown when input is not a valid chord.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when semitones parameter is a negative number.</exception>
        public static Chord TransposeUp(string chord, int semitones)
        {
            return TransposeUp(new Chord(chord), semitones);
        }

        /// <summary>
        /// Transposes up a chord.
        /// </summary>
        /// <param name="chord">The chord</param>
        /// <param name="semitones">The semitones to the transposition</param>
        /// <returns>A transposed chord.</returns>
        /// <exception cref="ArgumentNullException">Thrown when chord object is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when semitones parameter is a negative number.</exception>
        public static Chord TransposeUp(Chord chord, int semitones)
        {
            return Transpose(chord, semitones, TransposeUp);
        }

        /// <summary>
        /// Transposes up a note.
        /// </summary>
        /// <param name="note">The note</param>
        /// <param name="semitones">A transposed note.</param>
        /// <returns></returns>
        public static Note TransposeUp(Note note, int semitones)
        {
            var map = GetCorrectMap(_sharpMap, note);
            var initialChordNode = FindHeadNodeFromNote(map, note);

            while (semitones > 0)
            {
                if (initialChordNode.Right != null)
                {
                    initialChordNode = initialChordNode.Right;
                    semitones--;
                }
                else
                {
                    initialChordNode = initialChordNode.Down;
                }
            }

            if (initialChordNode.Note.IsSharpOrDoubleSharp)
            {
                if (initialChordNode.Down != null)
                {
                    initialChordNode = initialChordNode.Down;
                }
            }

            return initialChordNode.Note;
        }

        /// <summary>
        /// Transposes up a list of chords.
        /// </summary>
        /// <param name="chords">The chord list</param>
        /// <param name="semitones">The semitones to the transposition</param>
        /// <returns>A transposed chord list</returns>
        /// <exception cref="ArgumentNullException">Thrown when chords object is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when semitones parameter is a negative number.</exception>
        public static List<Chord> TransposeUp(List<Chord> chords, int semitones)
        {
            if (chords is null)
            {
                throw new ArgumentNullException(nameof(chords));
            }

            var transposedChords = new List<Chord>();

            foreach (var chord in chords)
            {
                transposedChords.Add(TransposeUp(chord, semitones));
            }

            return transposedChords;
        }

        /// <summary>
        /// Transposes down a chord.
        /// </summary>
        /// <param name="chord">The chord</param>
        /// <param name="semitones">The semitones to the transposition</param>
        /// <returns>A transposed chord.</returns>
        /// <exception cref="ArgumentNullException">Thrown when chord object is null.</exception>
        /// <exception cref="NotAChordException">Thrown when input is not a valid chord.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when semitones parameter is a negative number.</exception>
        public static Chord TransposeDown(string chord, int semitones)
        {
            return TransposeDown(new Chord(chord), semitones);
        }

        /// <summary>
        /// Transposes down a chord.
        /// </summary>
        /// <param name="chord">The chord</param>
        /// <param name="semitones">The semitones to the transposition</param>
        /// <returns>A transposed chord.</returns>
        /// <exception cref="ArgumentNullException">Thrown when chords object is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when semitones parameter is a negative number.</exception>
        public static Chord TransposeDown(Chord chord, int semitones)
        {
            return Transpose(chord, semitones, TransposeDown);
        }

        /// <summary>
        /// Transposes down a note.
        /// </summary>
        /// <param name="note">The note</param>
        /// <param name="semitones">The semitones</param>
        /// <returns>A transposed note.</returns>
        public static Note TransposeDown(Note note, int semitones)
        {
            var map = GetCorrectMap(_flatMap, note);
            var initialChordNode = FindHeadNodeFromNote(map, note);

            while (semitones > 0)
            {
                if (initialChordNode.Left != null)
                {
                    initialChordNode = initialChordNode.Left;
                    semitones--;
                }
                else
                {
                    initialChordNode = initialChordNode.Up;
                }
            }

            if (initialChordNode.Note.IsFlatOrDoubleFlat)
            {
                if (initialChordNode.Up != null)
                {
                    initialChordNode = initialChordNode.Up;
                }
            }

            return initialChordNode.Note;
        }

        /// <summary>
        /// Transposes down a list of chords.
        /// </summary>
        /// <param name="chords">The chord list</param>
        /// <param name="semitones">The semitones to the transposition</param>
        /// <returns>A transposed chord list.</returns>
        /// <exception cref="ArgumentNullException">Thrown when chords object is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when semitones parameter is a negative number.</exception>
        public static List<Chord> TransposeDown(List<Chord> chords, int semitones)
        {
            if (chords is null)
            {
                throw new ArgumentNullException(nameof(chords));
            }

            var transposedChords = new List<Chord>();

            foreach (var chord in chords)
            {
                transposedChords.Add(TransposeDown(chord, semitones));
            }

            return transposedChords;
        }

        /// <summary>
        /// Extract the chords from a input text.
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>A list of chords.</returns>
        public static List<Chord> ExtractChords(string text)
        {
            var words = text.Split(' ').ToList();
            words = words.Where(word => word != "").ToList();
            return GetValidChords(words.ToList());
        }

        /// <summary>
        /// Get valid chord from a chords string list.
        /// </summary>
        /// <param name="chords">The chords</param>
        /// <returns>A list of valid chords.</returns>
        public static List<Chord> GetValidChords(List<string> chords)
        {
            var validChords = new List<Chord>();

            foreach (var chord in chords)
            {
                if (IsChord(chord))
                {
                    validChords.Add(new Chord(chord));
                }
            }

            return validChords;
        }

        /// <summary>
        /// Get the number of semitones from a chord to another.
        /// </summary>
        /// <param name="from">The origin chord</param>
        /// <param name="to">The destiny chord</param>
        /// <returns>The number of semitones from a chord to another.</returns>
        public static int GetSemitones(Note from, Note to)
        {
            var (polarizedOrigin, polarizedDestiny) = MatchChromaticPole(from, to);

            var map = GetCorrectMap(_sharpMap, polarizedOrigin);
            map = GetCorrectMap(map, polarizedDestiny);

            var headNode = FindHeadNodeFromNote(map, polarizedOrigin);
            int semitones = 0;

            while (headNode.Note.ToString() != polarizedDestiny.ToString())
            {
                if (headNode.Right != null)
                {
                    headNode = headNode.Right;
                    semitones++;
                }
                else
                {
                    headNode = headNode.Down;
                }
            }

            return semitones;
        }

        /// <summary>
        /// Check if two chords have different chromatic poles.
        /// </summary>
        /// <param name="note1">The first chord</param>
        /// <param name="note2">The second chord</param>
        /// <returns>True if both have different chromatic poles.</returns>
        public static bool HasDifferentChromaticPole(Note note1, Note note2)
        {
            return (note1.IsFlatOrDoubleFlat && note2.IsSharpOrDoubleSharp) || (note1.IsSharpOrDoubleSharp && note2.IsFlatOrDoubleFlat);
        }

        /// <summary>
        /// Get the chromatic correspondent note (if it is flat, the correspondent will be sharp and vice versa).
        /// </summary>
        /// <param name="note">The note</param>
        /// <returns>The chromatic correspondent note.</returns>
        public static Note GetChromaticCorrespondent(Note note)
        {
            if (note.Accident == Accident.None)
            {
                return note;
            }

            return _chromaticCorrespondentDictionary[note];
        }

        /// <summary>
        /// Check if the given name is a valid chord.
        /// </summary>
        /// <param name="chordName">The chord name</param>
        /// <returns>True if the given name is a valid chord.</returns>
        public static bool IsChord(string chordName)
        {
            try
            {
                return new Chord(chordName) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Apply optmizations to the input chord.
        /// </summary>
        /// <param name="chord">The chord</param>
        /// <returns>The optmized chord.</returns>
        public static Chord Optimize(Chord chord)
        {
            return OptimizeInversion(chord);
        }
        #endregion

        #region Private Methods
        private static Chord Transpose(Chord chord, int semitones, TransposeMethod transposeMethod)
        {
            if (chord is null)
            {
                throw new ArgumentNullException(nameof(chord));
            }

            chord = Optimize(chord);
            semitones = NormalizeSemitones(semitones);

            if (semitones == 0)
            {
                return chord;
            }

            var transposedNote = transposeMethod(chord.Note, semitones);
            Note transposedInversion = null;

            if (chord.Inversion != null)
            {
                transposedInversion = transposeMethod(chord.Inversion, semitones);
            }

            return new Chord(transposedNote, chord.Tonality, chord.Complement, transposedInversion);
        }

        private static Chord OptimizeInversion(Chord chord)
        {
            Note note = chord.Note;
            Note inversion = chord.Inversion;

            if (chord is not null)
            {
                if (note is not null && inversion is not null)
                {
                    if (HasDifferentChromaticPole(chord.Note, chord.Inversion))
                    {
                        inversion = GetChromaticCorrespondent(chord.Inversion);
                    }

                    if (note.Equals(inversion))
                    {
                        inversion = null;
                    }
                }
            }

            return new Chord(note, chord.Tonality, chord.Complement, inversion);
        }

        private static int NormalizeSemitones(int semitones)
        {
            if (semitones < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(semitones), "Semitones can not be negative numbers");
            }

            return semitones % SemitonesOnTheScale;
        }

        private static Node GetCorrectMap(Node map, Note note)
        {
            return note switch
            {
                _ when note.IsFlatOrDoubleFlat   => _flatMap,
                _ when note.IsSharpOrDoubleSharp => _sharpMap,
                _                                => map
            };
        }

        private static Node FindHeadNodeFromNote(Node mapHeadNode, Note note)
        {
            mapHeadNode.HasVisited = true;

            if (mapHeadNode.Note.Equals(note))
            {
                mapHeadNode.HasVisited = false;
                return mapHeadNode;
            }

            Node node = null;

            if (mapHeadNode.Right != null && !mapHeadNode.Right.HasVisited)
            {
                node = FindHeadNodeFromNote(mapHeadNode.Right, note);
            }

            if (node is null && mapHeadNode.Down != null && !mapHeadNode.Down.HasVisited)
            {
                node = FindHeadNodeFromNote(mapHeadNode.Down, note);
            }

            mapHeadNode.HasVisited = false;

            return node;
        }

        private static (Note, Note) MatchChromaticPole(Note chord1, Note chord2)
        {
            var polarizedChord1 = chord1;
            var polarizedChord2 = chord2;

            if (polarizedChord1.IsDoubleFlat || polarizedChord1.IsDoubleSharp)
            {
                polarizedChord1 = GetChromaticCorrespondent(polarizedChord1);
            }

            if (polarizedChord2.IsDoubleFlat || polarizedChord2.IsDoubleSharp)
            {
                polarizedChord2 = GetChromaticCorrespondent(polarizedChord2);
            }

            if (HasDifferentChromaticPole(polarizedChord1, polarizedChord2))
            {
                if (polarizedChord1.IsFlat || polarizedChord1.IsSharp)
                {
                    polarizedChord1 = GetChromaticCorrespondent(polarizedChord1);
                }
                else if (polarizedChord2.IsFlat || polarizedChord2.IsSharp)
                {
                    polarizedChord2 = GetChromaticCorrespondent(polarizedChord2);
                }
            }

            return (polarizedChord1, polarizedChord2);
        }

        private static Node BuildFlatMap()
        {
            var head = new Node(new Note(NoteLetter.A, Accident.None));

            // A
            var current = head;
            current.Down = new Node(new Note(NoteLetter.B, Accident.DoubleFlat));
            current.Down.Up = current;

            // Bbb
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.B, Accident.Flat));
            current.Right.Left = current;

            // Bb
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.B, Accident.None));
            current.Down = new Node(new Note(NoteLetter.C, Accident.DoubleFlat));
            current.Right.Left = current;
            current.Down.Up = current;

            // Cbb
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.C, Accident.Flat));
            current.Right.Left = current;

            // Cb
            current = current.Right;
            current.Up = current.Left.Up.Right;
            current.Up.Down = current;
            current.Right = new Node(new Note(NoteLetter.C, Accident.None));
            current.Right.Left = current;

            // C
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.D, Accident.DoubleFlat));
            current.Down.Up = current;

            // Dbb
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.D, Accident.Flat));
            current.Right.Left = current;

            // Db
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.D, Accident.None));
            current.Right.Left = current;

            // D
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.E, Accident.DoubleFlat));
            current.Down.Up = current;

            // Ebb
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.E, Accident.Flat));
            current.Right.Left = current;

            // Eb
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.E, Accident.None));
            current.Right.Left = current;
            current.Down = new Node(new Note(NoteLetter.F, Accident.DoubleFlat));
            current.Down.Up = current;

            // Fbb
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.F, Accident.Flat));
            current.Right.Left = current;

            // Fb
            current = current.Right;
            current.Up = current.Left.Up.Right;
            current.Up.Down = current;
            current.Right = new Node(new Note(NoteLetter.F, Accident.None));
            current.Right.Left = current;

            // F
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.G, Accident.DoubleFlat));
            current.Down.Up = current;

            // Gbb
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.G, Accident.Flat));
            current.Right.Left = current;

            // Gb
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.G, Accident.None));
            current.Right.Left = current;

            // G
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.A, Accident.DoubleFlat));
            current.Down.Up = current;

            // Abb
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.A, Accident.Flat));
            current.Right.Left = current;

            // Ab
            current = current.Right;
            current.Right = head;
            current.Right.Left = current;

            return head;
        }

        private static Node BuildSharpMap()
        {
            var head = new Node(new Note(NoteLetter.A, Accident.None));

            // A
            var current = head;
            current.Right = new Node(new Note(NoteLetter.A, Accident.Sharp));
            current.Right.Left = current;

            // A#
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.A, Accident.DoubleSharp));
            current.Right.Left = current;

            // A##
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.B, Accident.None));
            current.Down.Up = current;

            // B
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.B, Accident.Sharp));
            current.Right.Left = current;

            // B#
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.B, Accident.DoubleSharp));
            current.Right.Left = current;
            current.Down = new Node(new Note(NoteLetter.C, Accident.None));
            current.Down.Up = current;

            // B##
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.C, Accident.Sharp));
            current.Down.Up = current;

            // C#
            current = current.Down;
            current.Left = current.Up.Left.Down;
            current.Left.Right = current;
            current.Right = new Node(new Note(NoteLetter.C, Accident.DoubleSharp));
            current.Right.Left = current;

            // C##
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.D, Accident.None));
            current.Down.Up = current;

            // D
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.D, Accident.Sharp));
            current.Right.Left = current;

            // D#
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.D, Accident.DoubleSharp));
            current.Right.Left = current;

            // D##
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.E, Accident.None));
            current.Down.Up = current;

            // E
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.E, Accident.Sharp));
            current.Right.Left = current;

            // E#
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.E, Accident.DoubleSharp));
            current.Right.Left = current;
            current.Down = new Node(new Note(NoteLetter.F, Accident.None));
            current.Down.Up = current;

            // E##
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.F, Accident.Sharp));
            current.Down.Up = current;

            // F#
            current = current.Down;
            current.Left = current.Up.Left.Down;
            current.Left.Right = current;
            current.Right = new Node(new Note(NoteLetter.F, Accident.DoubleSharp));
            current.Right.Left = current;

            // F##
            current = current.Right;
            current.Down = new Node(new Note(NoteLetter.G, Accident.None));
            current.Down.Up = current;

            // G
            current = current.Down;
            current.Right = new Node(new Note(NoteLetter.G, Accident.Sharp));
            current.Right.Left = current;

            // G#
            current = current.Right;
            current.Right = new Node(new Note(NoteLetter.G, Accident.DoubleSharp));
            current.Right.Left = current;

            // G##
            current = current.Right;
            current.Down = head;
            current.Down.Up = current;

            return head;
        }

        private static Dictionary<Note, Note> BuildChromaticCorrespondentDictionary()
        {
            return new Dictionary<Note, Note>
            {
                { new Note(NoteLetter.A, Accident.Flat), new Note(NoteLetter.G, Accident.Sharp) },
                { new Note(NoteLetter.G, Accident.Flat), new Note(NoteLetter.F, Accident.Sharp) },
                { new Note(NoteLetter.F, Accident.Flat), new Note(NoteLetter.E, Accident.None) },
                { new Note(NoteLetter.E, Accident.Flat), new Note(NoteLetter.D, Accident.Sharp) },
                { new Note(NoteLetter.D, Accident.Flat), new Note(NoteLetter.C, Accident.Sharp) },
                { new Note(NoteLetter.C, Accident.Flat), new Note(NoteLetter.B, Accident.None) },
                { new Note(NoteLetter.B, Accident.Flat), new Note(NoteLetter.A, Accident.Sharp) },
                { new Note(NoteLetter.A, Accident.Sharp), new Note(NoteLetter.B, Accident.Flat) },
                { new Note(NoteLetter.B, Accident.Sharp), new Note(NoteLetter.C, Accident.None) },
                { new Note(NoteLetter.C, Accident.Sharp), new Note(NoteLetter.D, Accident.Flat) },
                { new Note(NoteLetter.D, Accident.Sharp), new Note(NoteLetter.E, Accident.Flat) },
                { new Note(NoteLetter.E, Accident.Sharp), new Note(NoteLetter.F, Accident.None) },
                { new Note(NoteLetter.F, Accident.Sharp), new Note(NoteLetter.G, Accident.Flat) },
                { new Note(NoteLetter.G, Accident.Sharp), new Note(NoteLetter.A, Accident.Flat) },
                { new Note(NoteLetter.A, Accident.DoubleFlat), new Note(NoteLetter.G, Accident.None) },
                { new Note(NoteLetter.B, Accident.DoubleFlat), new Note(NoteLetter.A, Accident.None) },
                { new Note(NoteLetter.C, Accident.DoubleFlat), new Note(NoteLetter.B, Accident.Flat) },
                { new Note(NoteLetter.D, Accident.DoubleFlat), new Note(NoteLetter.C, Accident.None) },
                { new Note(NoteLetter.E, Accident.DoubleFlat), new Note(NoteLetter.D, Accident.None) },
                { new Note(NoteLetter.F, Accident.DoubleFlat), new Note(NoteLetter.E, Accident.Flat) },
                { new Note(NoteLetter.G, Accident.DoubleFlat), new Note(NoteLetter.F, Accident.None) },
                { new Note(NoteLetter.A, Accident.DoubleSharp), new Note(NoteLetter.B, Accident.None) },
                { new Note(NoteLetter.B, Accident.DoubleSharp), new Note(NoteLetter.C, Accident.Sharp) },
                { new Note(NoteLetter.C, Accident.DoubleSharp), new Note(NoteLetter.D, Accident.None) },
                { new Note(NoteLetter.D, Accident.DoubleSharp), new Note(NoteLetter.E, Accident.None) },
                { new Note(NoteLetter.E, Accident.DoubleSharp), new Note(NoteLetter.F, Accident.Flat) },
                { new Note(NoteLetter.F, Accident.DoubleSharp), new Note(NoteLetter.G, Accident.None) },
                { new Note(NoteLetter.G, Accident.DoubleSharp), new Note(NoteLetter.A, Accident.None) },
            };
        }
        #endregion

        #region Delegates
        private delegate Note TransposeMethod(Note note, int semitones);
        #endregion
    }
}
