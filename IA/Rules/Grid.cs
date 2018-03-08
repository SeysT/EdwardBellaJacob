using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    class Grid
    {
        public Grid(List<Pawn> pawns)
        {
            Pawns = pawns;
        }

        public Grid()
        {
            Pawns = new List<Pawn>();
        }

        public void Add(Pawn p)
        {
            Pawns.Add(p);
        }



        public void Move(Pawn p, Coord c)
        {
            // TODO
        }

        public List<Pawn> Pawns { get; private set; }
    }
}
