using SparksMusic.Library.Enum;
using System;

namespace SparksMusic.Library
{
    public static class Transposer
    {
        public static readonly Node FlatMap = BuildFlatMap();
        public static readonly Node SharpMap = BuildSharpMap();

        private const int SemitonesOnTheScale = 12;

        public static Chord TransposeUp(string chord, int semitones)
        {
            return TransposeUp(new Chord(chord), semitones);
        }

        public static Chord TransposeUp(Chord chord, int semitones)
        {
            semitones = NormalizeSemitones(semitones);

            var map = SharpMap;

            if (chord.Note.Accident == Accident.Flat || chord.Note.Accident == Accident.DoubleFlat)
            {
                map = FlatMap;
            }

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

            if (initialChordNode.Note.Accident == Accident.Sharp || initialChordNode.Note.Accident == Accident.DoubleSharp)
            {
                if (initialChordNode.Down != null)
                {
                    initialChordNode = initialChordNode.Down;
                }
            }

            return new Chord(initialChordNode.Note, chord.Tonality, chord.Complement, chord.Inversion);
        }

        public static Chord TransposeDown(string chord, int semitones)
        {
            return TransposeDown(new Chord(chord), semitones);
        }

        public static Chord TransposeDown(Chord chord, int semitones)
        {
            semitones = NormalizeSemitones(semitones);

            var map = FlatMap;

            if (chord.Note.Accident == Accident.Sharp || chord.Note.Accident == Accident.DoubleSharp)
            {
                map = SharpMap;
            }

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

            if (initialChordNode.Note.Accident == Accident.Flat || initialChordNode.Note.Accident == Accident.DoubleFlat)
            {
                if (initialChordNode.Up != null)
                {
                    initialChordNode = initialChordNode.Up;
                }
            }

            return new Chord(initialChordNode.Note, chord.Tonality, chord.Complement, chord.Inversion);
        }

        private static int NormalizeSemitones(int semitones)
        {
            if (semitones < 0)
                throw new ArgumentOutOfRangeException("Semitons não podem ser negativos.");

            return semitones % SemitonesOnTheScale;
        }

        private static Node FindHeadNodeFromNote(Node mapHeadNode, Note note)
        {
            mapHeadNode.HasVisited = true;

            if (mapHeadNode.Note.Equals(note))
            {
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



            return head;
        }
    }
}
