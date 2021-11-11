using SparksMusic.Library;
using SparksMusic.Library.Enum;
using SparksMusic.Library.Exceptions;
using Xunit;

namespace SparksMusic.Test
{
    public class ChordTest
    {
        [Fact]
        public void Should_ReturnTrue_When_CompareTwoEqualChords()
        {
            var noteLetter = NoteLetter.A;
            var accident = Accident.None;
            var tonality = Tonality.Major;
            var complement = "";
            var inversionNoteLetter = NoteLetter.D;
            var inversionAccident = Accident.Sharp;

            var chord1 = new Chord(
                new Note(noteLetter, accident),
                tonality,
                complement,
                new Note(inversionNoteLetter, inversionAccident));

            var chord2 = new Chord(
                new Note(noteLetter, accident),
                tonality,
                complement,
                new Note(inversionNoteLetter, inversionAccident));

            Assert.True(chord1.Equals(chord2));
        }

        [Fact]
        public void Should_ReturnFalse_When_CompareTwoDifferentChords()
        {
            var chord1 = new Chord(new Note(NoteLetter.A, Accident.Flat));
            var chord2 = new Chord(new Note(NoteLetter.A, Accident.DoubleFlat));

            Assert.False(chord1.Equals(chord2));
        }

        [Fact]
        public void Should_SayThatObjectsAreEquals_When_CreateChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.A, Accident.None);
            var tonality = Tonality.Major;
            var complement = "7M(b5)";
            var inversion = new Note(NoteLetter.D, Accident.Sharp);

            var chord = new Chord("A7M(b5)/D#");

            Assert.Equal(note, chord.Note);
            Assert.Equal(tonality, chord.Tonality);
            Assert.Equal(complement, chord.Complement);
            Assert.Equal(inversion, chord.Inversion);
            Assert.Equal("A7M(b5)/D#", chord.ToString());
        }

        [Fact]
        public void Should_ThrowsNotAChordException_When_CreateChordObjectWithInvalidString()
        {
            Assert.Throws<NotAChordException>(() => new Chord("H"));
        }

        [Fact]
        public void Should_SayThatObjectsAreDifferent_When_CreateChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.C, Accident.Sharp);
            var tonality = Tonality.Minor;
            var complement = "7(11)";
            var inversion = new Note(NoteLetter.D, Accident.Sharp);

            var chord = new Chord("Asus4/E");

            Assert.NotEqual(note, chord.Note);
            Assert.NotEqual(tonality, chord.Tonality);
            Assert.NotEqual(complement, chord.Complement);
            Assert.NotEqual(inversion, chord.Inversion);
        }

        [Fact]
        public void Should_SayThatObjectsAreEquals_When_CreateAugmentedChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.A, Accident.None);
            var tonality = Tonality.Augmented;
            var complement = "";
            Note inversion = null;

            var chord = new Chord("A+");

            Assert.Equal(note, chord.Note);
            Assert.Equal(tonality, chord.Tonality);
            Assert.Equal(complement, chord.Complement);
            Assert.Equal(inversion, chord.Inversion);
            Assert.Equal("A+", chord.ToString());
        }

        [Fact]
        public void Should_SayThatObjectsAreDifferent_When_CreateAugmentedChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.C, Accident.Sharp);
            var tonality = Tonality.Minor;
            var complement = "7(11)";
            var inversion = new Note(NoteLetter.D, Accident.Sharp);

            var chord = new Chord("A+");

            Assert.NotEqual(note, chord.Note);
            Assert.NotEqual(tonality, chord.Tonality);
            Assert.NotEqual(complement, chord.Complement);
            Assert.NotEqual(inversion, chord.Inversion);
        }

        [Fact]
        public void Should_SayThatObjectsAreEquals_When_CreateDiminutedChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.A, Accident.None);
            var tonality = Tonality.Diminuted;
            var complement = "";
            Note inversion = null;

            var chord = new Chord("A°");

            Assert.Equal(note, chord.Note);
            Assert.Equal(tonality, chord.Tonality);
            Assert.Equal(complement, chord.Complement);
            Assert.Equal(inversion, chord.Inversion);
            Assert.Equal("A°", chord.ToString());
        }

        [Fact]
        public void Should_SayThatObjectsAreDifferent_When_CreateDiminutedChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.C, Accident.Sharp);
            var tonality = Tonality.Minor;
            var complement = "7(11)";
            var inversion = new Note(NoteLetter.D, Accident.Sharp);

            var chord = new Chord("A°");

            Assert.NotEqual(note, chord.Note);
            Assert.NotEqual(tonality, chord.Tonality);
            Assert.NotEqual(complement, chord.Complement);
            Assert.NotEqual(inversion, chord.Inversion);
        }

        [Fact]
        public void Should_SayThatObjectsAreEquals_When_CreateHalfDiminutedChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.A, Accident.None);
            var tonality = Tonality.HalfDiminuted;
            var complement = "";
            Note inversion = null;

            var chord = new Chord("Am7(b5)");

            Assert.Equal(note, chord.Note);
            Assert.Equal(tonality, chord.Tonality);
            Assert.Equal(complement, chord.Complement);
            Assert.Equal(inversion, chord.Inversion);
            Assert.Equal("Am7(b5)", chord.ToString());
        }

        [Fact]
        public void Should_SayThatObjectsAreDifferent_When_CreateHalfDiminutedChordObjectWithValidString()
        {
            var note = new Note(NoteLetter.C, Accident.Sharp);
            var tonality = Tonality.Minor;
            var complement = "7(11)";
            var inversion = new Note(NoteLetter.D, Accident.Sharp);

            var chord = new Chord("Am7(b5)");

            Assert.NotEqual(note, chord.Note);
            Assert.NotEqual(tonality, chord.Tonality);
            Assert.NotEqual(complement, chord.Complement);
            Assert.NotEqual(inversion, chord.Inversion);
        }

        [Fact]
        public void Should_UpASemitoneToTheChord_When_CallIncrementOperator()
        {
            var chord = new Chord("Abm7(b5)");
            chord++;

            Assert.Equal("Am7(b5)", chord.ToString());
        }

        [Fact]
        public void Should_DownASemitoneToTheChord_When_CallDecrementOperator()
        {
            var chord = new Chord("Abm7(b5)");
            chord--;

            Assert.Equal("Gm7(b5)", chord.ToString());
        }
    }
}
