using System;
using SparksMusic.Library;
using SparksMusic.Library.Enum;
using Xunit;

namespace Transposer.Test
{
    public class NoteTest
    {
        [Fact]
        public void Should_ReturnTrue_When_CompareTwoEqualNotes()
        {
            var noteLetter = NoteLetter.A;
            var accident = Accident.None;

            var note1 = new Note(noteLetter, accident);
            var note2 = new Note(noteLetter, accident);

            Assert.True(note1.Equals(note2));
        }

        [Fact]
        public void Should_ReturnFalse_When_CompareTwoDifferentNotes()
        {
            var note1 = new Note(NoteLetter.A, Accident.Flat);
            var note2 = new Note(NoteLetter.A, Accident.DoubleFlat);

            Assert.False(note1.Equals(note2));
        }
    }
}
