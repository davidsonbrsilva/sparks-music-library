using SparksMusic.Library;
using Xunit;

namespace SparksMusic.Test
{
    public class NodeTest
    {
        [Fact]
        public void Should_SayThatObjectNameIsEqualToExpected_When_CreateANodeObject()
        {
            var node = new Node(new Note(Library.Enum.NoteLetter.A, Library.Enum.Accident.DoubleFlat));
            Assert.Equal("Abb", node.ToString());
        }
    }
}
