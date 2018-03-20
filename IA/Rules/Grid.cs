using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    public class Grid
    {
        public List<Pawn> Pawns { get; private set; }

        public Grid()
        {
            this.Pawns = new List<Pawn>();
        }

        public Grid(Grid g)
        {
            this.Pawns = new List<Pawn>(g.Pawns);
        }

        public Grid(List<Pawn> pawns)
        {
            Pawns = pawns;
        }

        public void Add(Pawn p)
        {
            Pawns.Add(p);
        }

        /// <summary>
        /// Renvoie ce qu'il y a dans une case, envoie pawn(h , 0, coord) par défaut
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Pawn GetInCoord(Coord c)
        {
            foreach (var p in Pawns)
            {
                if (c.Equals(p.Coordinates))
                {
                    return new Pawn(p);
                }
            }
            return new Pawn(Race.HUM, 0, c);
        }

        /// <summary>
        /// Permet d'update la quantité du pawn de coordonnées c dans la grille.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="quantity"></param>
        /// <returns>return true in a pawn has been updated</returns>
        public bool SetQuantityInCoord(Coord c, float quantity)
        {
            foreach (var pawn in Pawns)
            {
                if (c.Equals(pawn.Coordinates))
                {
                    Pawns.Remove(pawn);
                    if (quantity > 0)
                    {
                        Pawns.Add(new Pawn(pawn.Race, quantity, pawn.Coordinates));
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This function updates quantity in coords if it exists or create with given quantity and race
        /// </summary>
        /// <param name="c">coord to look</param>
        /// <param name="quantity">quantity to set</param>
        /// <param name="race">race to add if pawn does not already exist</param>
        /// <returns>true if pawn has been updated false if it has been created</returns>
        public bool SetQuantityInCoord(Coord c, int quantity, Race race)
        {
            foreach (var pawn in Pawns)
            {
                if (c.Equals(pawn.Coordinates))
                {
                    Pawns.Remove(pawn);
                    if (quantity > 0)
                    {
                        Pawns.Add(new Pawn(race, quantity, c));
                    }
                    return true;
                }
            }
            Pawns.Add(new Pawn(race, quantity, c));
            return false;
        }
    }
}
