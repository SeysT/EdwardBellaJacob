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

        public Pawn getInCoord(Coord c)
        {
            ///<summary>
            ///Renvoie ce qu'il y a dans une case, envoie pawn(h , 0 ,coord) par défaut
            ///</summary>
            foreach (var p in Pawns)
            {
                if (c.Equals(p.Coordinates))
                {
                    return p;
                }
            }
            return new Pawn('h', 0, c);
        }

        public void Move(Pawn p, Coord c)
        {
            // TODO
        }

        public List<Pawn> Pawns { get; private set; }
    }
}
