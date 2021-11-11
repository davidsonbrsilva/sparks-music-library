using SparksMusic.Library;
using System;

namespace ApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Transposer.TransposeDown("H", 3));
        }
    }
}
