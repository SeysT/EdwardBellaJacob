using IA.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    // class to compute scores
    // For all modifications, ask Anouar and Baudouin

    class Heuristic
    {
        public Heuristic(Board board)
        {
            this._board = board;
        }

        private Board _board;

        float OurStrengthHeuristic {
            get
            {
                return _board.OurNumber();
            }
        }

        float EnnemyStrengthHeuristic {
            get {
                return _board.EnnemyNumber();
            }
        }

        float BiggestGroupHeuristic
        {
            get {
                List<int> OurPositions = new List<int>();
                foreach (KeyValuePair<Coord, int> keyPair in _board.OurPositions())
                {
                    OurPositions.Add((keyPair.Value));
                }

                List<int> EnnemyPositions = new List<int>();
                foreach (KeyValuePair<Coord, int> keyPair in _board.EnnemyPositions())
                {
                    EnnemyPositions.Add((keyPair.Value));
                }

                return OurPositions.Max() - EnnemyPositions.Max();
            }
        }

        float DensityHeuristic
        {
            get
            {
                return _board.OurNumber() / _board.OurPositions().Count - _board.EnnemyNumber() / _board.EnnemyPositions().Count;
            }
        }

        float DispersionHeuristic
        {
            get
            {
                float[] OurBarycentre = new float[2];
                float[] EnnemyBarycentre = new float[2];

                int numerateurX = 0;
                int numerateurY = 0;
                int denominateur = _board.OurNumber();
                foreach (KeyValuePair<Coord, int> kvp in _board.OurPositions())
                {
                    numerateurX += kvp.Key.X * kvp.Value;
                    numerateurY += kvp.Key.Y * kvp.Value;
                }
                OurBarycentre[0] = numerateurX / denominateur;
                OurBarycentre[1] = numerateurY / denominateur;

                int enNumerateurX = 0;
                int enNumerateurY = 0;
                int enDenominateur = _board.EnnemyNumber();
                foreach (KeyValuePair<Coord, int> kvp in _board.EnnemyPositions())
                {
                    enNumerateurX += kvp.Key.X * kvp.Value;
                    enNumerateurY += kvp.Key.Y * kvp.Value;
                }
                EnnemyBarycentre[0] = enNumerateurX / enDenominateur;
                EnnemyBarycentre[1] = enNumerateurY / enDenominateur;

                float OurDistance = 0;
                float EnnemyDistance = 0;
                foreach (KeyValuePair<Coord, int> keyPair in _board.OurPositions())
                {
                    OurDistance += Math.Max(keyPair.Key.X - OurBarycentre[0], keyPair.Key.Y - OurBarycentre[1]);
                }
                foreach (KeyValuePair<Coord, int> keyPair in _board.EnnemyPositions())
                {
                    EnnemyDistance += Math.Max(keyPair.Key.X - EnnemyBarycentre[0], keyPair.Key.Y - EnnemyBarycentre[1]);
                }
                OurDistance = OurDistance / _board.OurPositions().Count;
                EnnemyDistance = EnnemyDistance / _board.EnnemyPositions().Count();

                return OurDistance - EnnemyDistance;
            }
        }

        public float GetScore(float a11, float a12, float a2, float a3, float a4)
        {
            return (
                a11 * this.OurStrengthHeuristic -
                a12 * this.EnnemyStrengthHeuristic + 
                a2 * this.BiggestGroupHeuristic +
                a3 * this.DensityHeuristic + 
                a4 * this.DispersionHeuristic
            );
        }

    }
}
