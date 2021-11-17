using SparksMusic.Library;
using Xunit;

namespace SparksMusic.Test
{
    public class NodeTest
    {
        [Fact]
        public void Should_SayThatObjectNameIsEqualToExpected_When_CreateANodeObject()
        {
            var node = new Node(new Note(NoteLetter.A, Accident.DoubleFlat));
            Assert.Equal("Abb", node.ToString());
        }
    }
}
