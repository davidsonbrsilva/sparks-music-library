using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SparksMusic.Test")]

namespace SparksMusic.Library
{
    class Node
    {
        public Node Up { get; set; }
        public Node Right { get; set; }
        public Node Down { get; set; }
        public Node Left { get; set; }
        public Note Note { get; set; }
        public bool HasVisited { get; set; }

        public Node(Note note, Node up = null, Node right = null, Node down = null, Node left = null)
        {
            Note = note;
            Up = up;
            Right = right;
            Down = down;
            Left = left;
        }

        public override string ToString()
        {
            return Note.ToString();
        }
    }
}
