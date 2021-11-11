using SparksMusic.Library;
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
        [InlineData("A#", 3, "G")]
        [InlineData("A##", 3, "D")]
        [InlineData("A##", 12, "A##")]
        [InlineData("A##", 13, "C")]
        [InlineData("A##", 11, "A#")]
        public void Should_TransposeChordUp_When_CallTransposeUpMethodPassingAValidChordAsArgument(string chord, int semitones, string expected)
        {
            Assert.Equal(expected, Transposer.TransposeUp(chord, semitones).ToString());
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
    }
}
