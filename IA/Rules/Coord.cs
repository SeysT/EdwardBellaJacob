using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    class Coord
    {
        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coord(int[,] coords)
        {
            X = coords[0, 0];
            Y = coords[1, 0];
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var coord = obj as Coord;
            return coord != null &&
                   X == coord.X &&
                   Y == coord.Y;
        }
    }
}
