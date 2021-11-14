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
            if (chord is null)
            {
                throw new ArgumentNullException(nameof(chord));
            }

            semitones = NormalizeSemitones(semitones);

            if (semitones == 0)
            {
                return chord;
            }

            var map = GetCorrectMap(_sharpMap, chord.Note);

            var initialChordNode = FindHeadNodeFromNote(map, chord.Note);

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

            return new Chord(initialChordNode.Note, chord.Tonality, chord.Complement, chord.Inversion);
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
            if (chord is null)
            {
                throw new ArgumentNullException(nameof(chord));
            }

            semitones = NormalizeSemitones(semitones);

            if (semitones == 0)
            {
                return chord;
            }

            var map = GetCorrectMap(_flatMap, chord.Note);

            var initialChordNode = FindHeadNodeFromNote(map, chord.Note);

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

            return new Chord(initialChordNode.Note, chord.Tonality, chord.Complement, chord.Inversion);
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
            return (note1.IsFlat && note2.IsSharp) || (note1.IsSharp && note2.IsFlat);
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
        /// Get interval from semitones.
        /// </summary>
        /// <param name="semitones">The semitones</param>
        /// <returns>The interval.</returns>
        public static string GetInterval(int semitones)
        {
            semitones = NormalizeSemitones(semitones);

            return semitones switch
            {
                0  => "pft8 or dim2",
                1  => "min2 or aug8",
                2  => "maj2 or dim3",
                3  => "min3 or aug2",
                4  => "maj3 or dim4",
                5  => "pft4 or aug3",
                6  => "aug4 or dim5",
                7  => "pft5 or dim6",
                8  => "min6 or aug5",
                9  => "maj6 or dim7",
                10  => "min7 or aug6",
                11 => "maj7 or dim8",
                _  => null
            };
        }

        /// <summary>
        /// Get interval from a note to other.
        /// </summary>
        /// <param name="from">The start note</param>
        /// <param name="to">The end note</param>
        /// <returns>The interval.</returns>
        public static string GetInterval(Note from, Note to)
        {
            int semitones = GetSemitones(from, to);
            return GetInterval(semitones);
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
                _                                 => map
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
    }
}
