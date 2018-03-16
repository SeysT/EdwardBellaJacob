﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    enum Race { US, THEM, HUM }

    class Pawn
    {
        public int Quantity { get; private set; }
        public Race Race { get; private set; }
        public Coord Coordinates { get; private set; }

        public Pawn(Race race, int quantity, Coord c)
        {
            Race = race;
            Quantity = quantity;
            Coordinates = new Coord(c);
        }

        public Pawn(Race race, int quantity, int x, int y)
        {
            Race = race;
            Quantity = quantity;
            Coordinates = new Coord(x, y);
        }
        
        public Pawn(Pawn p)
        {
            Race = p.Race;
            Quantity = p.Quantity;
            Coordinates = new Coord(p.Coordinates);
        }
    }
}
