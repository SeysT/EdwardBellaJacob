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
            _type = type;
            _quantity = quantity;
            _coordinates = c;
        }

        public Pawn(char type, int quantity, int x, int y)
        {
            _type = type;
            _quantity = quantity;
            _coordinates = new Coord(x,y);
        }
        
        public Pawn(Pawn p)
        {
            _type = p._type;
            _quantity = p._quantity;
            _coordinates = p._coordinates;
        }

        public int _quantity { get; private set; }
        public char _type { get; private set; }
        public Coord _coordinates { get; private set; }
    }
}
