using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    public class Grid
    {
        private Dictionary<Coord, Pawn> _pawns;

        public Grid()
        {
            this._pawns = new Dictionary<Coord, Pawn>();
        }

        public Grid(Grid g)
        {
            this._pawns = new Dictionary<Coord, Pawn>(g._pawns);
        }

        public Grid(Dictionary<Coord, Pawn> pawns)
        {
            this._pawns = pawns;
        }

        public List<Pawn> GetPawns()
        {
            return new List<Pawn> (this._pawns.Values);
        }

        public void Add(Pawn p)
        {
            if (!_pawns.ContainsKey(p.Coordinates))
                this._pawns.Add(p.Coordinates, p);
            else
                this._pawns[p.Coordinates] = p;
        }

        /// <summary>
        /// Renvoie ce qu'il y a dans une case, envoie pawn(h , 0, coord) par défaut
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Pawn GetInCoord(Coord c)
        {
            if (this._pawns.TryGetValue(c, out Pawn pawn))
            {
                return pawn;
            }
            return new Pawn(Race.HUM, 0, c);
        }

        /// <summary>
        /// Permet d'update la quantité du pawn de coordonnées c dans la grille.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="quantity"></param>
        /// <returns>return true in a pawn has been updated</returns>
        public bool SetQuantityInCoord(Coord c, int quantity)
        {
            if (this._pawns.TryGetValue(c, out Pawn pawn))
            {
                this._pawns.Remove(c);
                if (quantity > 0)
                {
                    _pawns.Add(c, new Pawn(pawn.Race, quantity, pawn.Coordinates));
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de remove le pawn d coordonnées c dans la grille.
        /// </summary>
        public void Remove(Coord c)
        {
            this._pawns.Remove(c);
        }

        /// <summary>
        /// Remove le pawn de la grille.
        /// </summary>
        public void Remove(Pawn pawn)
        {
            this.Remove(pawn.Coordinates);
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
            if (this._pawns.TryGetValue(c, out Pawn pawn)) {
                this._pawns.Remove(c);
                if (quantity > 0)
                {
                    this._pawns.Add(c, new Pawn(race, quantity, c));
                }
                return true;
            }
            this._pawns.Add(c, new Pawn(race, quantity, c));
            return false;
        }
    }
}
