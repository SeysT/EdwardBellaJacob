using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    enum Type { US, THEM, HUM }

    class Pawn
    {
        public int Quantity { get; private set; }
        public Type Type { get; private set; }
        public Coord Coordinates { get; private set; }

        public Pawn(Type type, int quantity, Coord c)
        {
            Type = type;
            Quantity = quantity;
            Coordinates = new Coord(c);
        }

        public Pawn(Type type, int quantity, int x, int y)
        {
            Type = type;
            Quantity = quantity;
            Coordinates = new Coord(x, y);
        }
        
        public Pawn(Pawn p)
        {
            Type = p.Type;
            Quantity = p.Quantity;
            Coordinates = new Coord(p.Coordinates);
        }
    }
}
