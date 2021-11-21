using SparksMusic.Library;
using System;
using System.Collections.Generic;
using Xunit;

namespace SparksMusic.Test
{
    public class TransposerTest
    {
        [Theory]
        [InlineData("A", 2, "B")]
        [InlineData("A", 3, "C")]
        [InlineData("Ab", 3, "B")]
        [InlineData("Abb", 3, "Bb")]
        [InlineData("Abb", 12, "Abb")]
        [InlineData("Abb", 13, "Ab")]
        [InlineData("Abb", 11, "Gb")]
        [InlineData("Cbb", 3, "Db")]
        [InlineData("A#", 3, "C#")]
        [InlineData("A##", 3, "D")]
        [InlineData("A##", 12, "A##")]
        [InlineData("A##", 13, "C")]
        [InlineData("A##", 11, "A#")]
        [InlineData("A#/C#", 2, "C/D#")]
        [InlineData("A#/Db", 2, "C/D#")]
        [InlineData("A#/Bb", 0, "A#")]
        [InlineData("A#/Bb", 2, "C")]
        public void Should_TransposeChordUp_When_CallTransposeUpMethodPassingAValidChordAsArgument(string chordName, int semitones, string expected)
        {
            var chord = new Chord(chordName);
            var chordList = new List<Chord>() { chord };

            Assert.Equal(expected, Transposer.TransposeUp(chord, semitones).ToString());
            Assert.Equal(expected, Transposer.TransposeUp(chordName, semitones).ToString());
            Assert.Equal(expected, Transposer.TransposeUp(chordList, semitones)[0].ToString());
        }

        [Fact]
        public void Should_ThrowArgumentNullException_When_CallTransposeUpMethodPassingANullChordAsArgument()
        {
            Assert.Throws<ArgumentNullException>(() => Transposer.TransposeUp((string)null, 2));
            Assert.Throws<ArgumentNullException>(() => Transposer.TransposeUp((Chord)null, 2));
            Assert.Throws<ArgumentNullException>(() => Transposer.TransposeUp((List<Chord>)null, 2));
        }

        [Fact]
        public void Should_ThrowArgumentOutOfRangeException_When_CallTransposeUpMethodPassingANegativeSemitoneAsArgument()
        {
            var chordName = "A#";
            var chord = new Chord(chordName);
            var chordList = new List<Chord>() { chord };

            Assert.Throws<ArgumentOutOfRangeException>(() => Transposer.TransposeUp(chord, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Transposer.TransposeUp(chordName, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Transposer.TransposeUp(chordList, -1));
        }

        [Theory]
        [InlineData("A", 2, "G")]
        [InlineData("A", 3, "Gb")]
        [InlineData("Ab", 3, "F")]
        [InlineData("Abb", 3, "E")]
        [InlineData("Abb", 12, "Abb")]
        [InlineData("Abb", 13, "Gb")]
        [InlineData("Abb", 11, "Ab")]
        [InlineData("Cbb", 3, "G")]
        [InlineData("A#", 3, "G")]
        [InlineData("A##", 3, "G#")]
        [InlineData("A##", 12, "A##")]
        [InlineData("A##", 13, "A#")]
        [InlineData("A##", 11, "C")]
        [InlineData("A#/C#", 2, "G#/B")]
        [InlineData("A#/Db", 2, "G#/B")]
        [InlineData("A#/Bb", 0, "A#")]
        [InlineData("A#/Bb", 2, "G#")]
        [InlineData("A##/Dbb", 2, "A/Bb")]
        public void Should_TransposeChordDown_When_CallTransposeUpMethodPassingAValidChordAsArgument(string chord, int semitones, string expected)
        {
            Assert.Equal(expected, Transposer.TransposeDown(chord, semitones).ToString());
        }

        [Fact]
        public void Should_ThrowArgumentNullException_When_CallTransposeDownMethodPassingANullChordAsArgument()
        {
            Assert.Throws<ArgumentNullException>(() => Transposer.TransposeDown((string)null, 2));
            Assert.Throws<ArgumentNullException>(() => Transposer.TransposeDown((Chord)null, 2));
            Assert.Throws<ArgumentNullException>(() => Transposer.TransposeDown((List<Chord>)null, 2));
        }

        [Fact]
        public void Should_ThrowArgumentOutOfRangeException_When_CallTransposeDownMethodPassingANegativeSemitoneAsArgument()
        {
            var chordName = "A#";
            var chord = new Chord(chordName);
            var chordList = new List<Chord>() { chord };

            Assert.Throws<ArgumentOutOfRangeException>(() => Transposer.TransposeDown(chord, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Transposer.TransposeDown(chordName, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Transposer.TransposeDown(chordList, -1));
        }

        [Theory]
        [InlineData("A")]
        [InlineData(" A")]
        [InlineData("A ")]
        [InlineData(" A ")]
        [InlineData("A#")]
        [InlineData("A##")]
        [InlineData("Ab")]
        [InlineData("Abb")]
        [InlineData("Am")]
        [InlineData("A#m")]
        [InlineData("A##m")]
        [InlineData("Abm")]
        [InlineData("Abbm")]
        [InlineData("A7")]
        [InlineData("Am7")]
        [InlineData("Am7M")]
        [InlineData("Am7(b5)")]
        [InlineData("A/G")]
        [InlineData("A#m7(b5,11)")]
        [InlineData("A#m(b5,11,13)/G#")]
        [InlineData("A+")]
        [InlineData("A\u00B0")]
        public void Should_ReturnTrue_When_CallIsValidMethodPassingAValidChordName(string chordName)
        {
            Assert.True(Transposer.IsChord(chordName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("H")]
        [InlineData("A###")]
        [InlineData("Abbb")]
        [InlineData("A#b")]
        [InlineData("Ab#")]
        [InlineData("AbM")]
        [InlineData("A+\u00B0")]
        [InlineData("A\u00B0+")]
        [InlineData("A(b5")]
        [InlineData("Ab5)")]
        [InlineData("A+(b5)")]
        [InlineData("Am(b5,)")]
        [InlineData("A/")]
        [InlineData("A/GF")]
        [InlineData("AG")]
        public void Should_ReturnFalse_When_CallIsValidMethodPassingAInvalidChordName(string chordName)
        {
            Assert.False(Transposer.IsChord(chordName));
        }

        [Theory]
        [InlineData(4, "A E    D  A")]
        [InlineData(5, "A#m7 E(b5)    D\u00B0  A## G")]
        [InlineData(4, "A#m7 E(b5)    H\u00B0  A## G")]
        public void Should_GetAListOfChord_When_CallExtractChordsMethodPassingAStringWithValidChords(int expectedNumberOfChords, string chordsLine)
        {
            Assert.Equal(expectedNumberOfChords, Transposer.ExtractChords(chordsLine).Count);
        }

        [Fact]
        public void Should_GetAListOfValidChord_When_CallGetValidChordsMethodPassingAChordsList()
        {
            var chords = new List<string>() { "Abm", "D(b5,9)", "H(4)", "E(4)" };
            var validChords = Transposer.GetValidChords(chords);

            Assert.Equal(3, validChords.Count);
            Assert.Equal("Abm", validChords[0].ToString());
            Assert.Equal("D(b5,9)", validChords[1].ToString());
            Assert.Equal("E(4)", validChords[2].ToString());
        }

        [Theory]
        [InlineData(0, NoteLetter.C, Accident.None, NoteLetter.C, Accident.None)]
        [InlineData(1, NoteLetter.C, Accident.None, NoteLetter.C, Accident.Sharp)]
        [InlineData(11, NoteLetter.C, Accident.None, NoteLetter.C, Accident.Flat)]
        [InlineData(7, NoteLetter.C, Accident.Sharp, NoteLetter.A, Accident.Flat)]
        public void Should_ReturnAmountOfSemitones_When_CallGetSemitonesMethodPassingTheOriginAndDestinyNotes(int expectedSemitones, NoteLetter originNoteLetter, Accident originAccident, NoteLetter destinyNoteLetter, Accident destinyAccident)
        {
            Assert.Equal(expectedSemitones, Transposer.GetSemitones(new Note(originNoteLetter, originAccident), new Note(destinyNoteLetter, destinyAccident)));
        }

        [Theory]
        [InlineData(NoteLetter.C, Accident.None, NoteLetter.C, Accident.None)]
        [InlineData(NoteLetter.C, Accident.None, NoteLetter.C, Accident.Flat)]
        [InlineData(NoteLetter.C, Accident.Flat, NoteLetter.C, Accident.None)]
        [InlineData(NoteLetter.C, Accident.Flat, NoteLetter.C, Accident.Flat)]
        [InlineData(NoteLetter.C, Accident.None, NoteLetter.C, Accident.Sharp)]
        [InlineData(NoteLetter.C, Accident.Sharp, NoteLetter.C, Accident.None)]
        [InlineData(NoteLetter.C, Accident.Sharp, NoteLetter.C, Accident.Sharp)]
        public void Should_ReturnFalse_When_CallHasDifferentChromaticPoleMethodPassingTwoNotesWithSameChromaticPoles(NoteLetter originNoteLetter, Accident originAccident, NoteLetter destinyNoteLetter, Accident destinyAccident)
        {
            Assert.False(Transposer.HasDifferentChromaticPole(new Note(originNoteLetter, originAccident), new Note(destinyNoteLetter, destinyAccident)));
        }

        [Theory]
        [InlineData(NoteLetter.C, Accident.Flat, NoteLetter.C, Accident.Sharp)]
        [InlineData(NoteLetter.C, Accident.Flat, NoteLetter.C, Accident.DoubleSharp)]
        [InlineData(NoteLetter.C, Accident.Sharp, NoteLetter.C, Accident.Flat)]
        [InlineData(NoteLetter.C, Accident.Sharp, NoteLetter.C, Accident.DoubleFlat)]
        [InlineData(NoteLetter.C, Accident.DoubleFlat, NoteLetter.C, Accident.Sharp)]
        [InlineData(NoteLetter.C, Accident.DoubleFlat, NoteLetter.C, Accident.DoubleSharp)]
        [InlineData(NoteLetter.C, Accident.DoubleSharp, NoteLetter.C, Accident.Flat)]
        [InlineData(NoteLetter.C, Accident.DoubleSharp, NoteLetter.C, Accident.DoubleFlat)]
        public void Should_ReturnTrue_When_CallHasDifferentChromaticPoleMethodPassingTwoNotesWithDifferentChromaticPoles(NoteLetter originNoteLetter, Accident originAccident, NoteLetter destinyNoteLetter, Accident destinyAccident)
        {
            Assert.True(Transposer.HasDifferentChromaticPole(new Note(originNoteLetter, originAccident), new Note(destinyNoteLetter, destinyAccident)));
        }

        [Theory]
        [InlineData("A#", NoteLetter.B, Accident.Flat)]
        [InlineData("Bb", NoteLetter.A, Accident.Sharp)]
        [InlineData("A", NoteLetter.A, Accident.None)]
        [InlineData("B", NoteLetter.A, Accident.DoubleSharp)]
        [InlineData("A", NoteLetter.B, Accident.DoubleFlat)]
        public void Should_GetChromaticCorrespondent_When_CallGetChromaticCorrespondentMethodPassingANote(string expected, NoteLetter noteLetter, Accident accident)
        {
            Assert.Equal(expected, Transposer.GetChromaticCorrespondent(new Note(noteLetter, accident)).ToString());
        }

        [Fact]
        public void Should_ThrowArgumentNullException_When_CallGetChromaticCorrespondentMethodPassingANullNote()
        {
            Assert.Throws<ArgumentNullException>(() => Transposer.GetChromaticCorrespondent(null));
        }

        [Theory]
        [InlineData("A", "A")]
        [InlineData("A#", "A#")]
        [InlineData("Ab", "Ab")]
        [InlineData("A#/Db", "A#/C#")]
        [InlineData("Ab/C#", "Ab/Db")]
        [InlineData("A/Ebb", "A/D")]
        [InlineData("A/D##", "A/E")]
        [InlineData("A#/Ebb", "A#/D")]
        [InlineData("A#/D##", "A#/E")]
        [InlineData("A##/Ebb", "B/D")]
        [InlineData("A##/D##", "B/E")]
        [InlineData("Abb/Ebb", "G/D")]
        [InlineData("Abb/Dbb", "G/C")]
        public void Should_GetOptimizedChord_When_CallOptimizeMethodPassingAChord(string chord, string expected)
        {
            Assert.Equal(expected, Transposer.Optimize(chord).ToString());
            Assert.Equal(expected, Transposer.Optimize(new Chord(chord)).ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData("H")]
        public void Should_ThrowArgumentNullException_When_CallOptimizeMethod_PassingAInvalidChordString(string chord)
        {
            Assert.Throws<NotAChordException>(() => Transposer.Optimize(chord));
        }

        [Fact]
        public void Should_ThrowArgumentNullException_When_CallOptimizeMethod_PassingANullChord()
        {
            Assert.Throws<ArgumentNullException>(() => Transposer.Optimize((string)null));
            Assert.Throws<ArgumentNullException>(() => Transposer.Optimize((Chord)null));
        }
    }
}
