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
        private int _h11 = 0;
        private int _h12 = 0;
        private int _h2 = 0;
        private int _h3 = 0;
        private float _h4 = 0;

        protected virtual void OurStrengthHeuristic()
        {
            _h11 = _board.OurNumber();
        }

        protected virtual void EnnemyStrengthHeuristic()
        {
            _h12 = _board.EnnemyNumber();
        }

        protected virtual void BiggestGroupHeuristic()
        {
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

            _h2 = OurPositions.Max() - EnnemyPositions.Max();
        }

        protected virtual void DensityHeuristic()
        {
            _h3 = _board.OurNumber() / _board.OurPositions().Count - _board.EnnemyNumber() / _board.EnnemyPositions().Count;
        }

        protected virtual void DispersionHeuristic()
        {
            List<float> OurBarycentre = new List<float>();
            List<float> EnnemyBarycentre = new List<float>();

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

            _h4 = OurDistance - EnnemyDistance;
        }

        public float Score(float a11, float a12, float a2, float a3, float a4)
        {
            return a11 * _h11 - a12 * _h12 + a2 * _h2 + a3 * _h3 + a4 * _h4;
        }

    }
}
