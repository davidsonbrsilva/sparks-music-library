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
        [InlineData("A°")]
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
        [InlineData("A+°")]
        [InlineData("A°+")]
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
    }
}
