using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    class Pawn
    {
        public Pawn(char type, int quantity, Coord c)
        {
            Type = type;
            Quantity = quantity;
            Coordinates = c;
        }

        public Pawn(char type, int quantity, int x, int y)
        {
            Type = type;
            Quantity = quantity;
            Coordinates = new Coord(x,y);
        }
        
        public int Quantity { get; private set; }
        public char Type { get; private set; }
        public Coord Coordinates { get; private set; }
    }
}
