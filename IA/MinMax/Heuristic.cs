using IA.Rules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    // class to compute scores
    // For all modifications, ask Anouar and Baudouin

    public class Heuristic
    {
        private Board _board;

        public Heuristic(Board board)
        {
            this._board = board;
        }

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
                if (OurPositions.Count.Equals(0))
                {
                    return 0;
                }
                else if (EnnemyPositions.Count.Equals(0))
                {
                    return 1;
                }
                return OurPositions.Max() - EnnemyPositions.Max();
            }
        }

        float DensityHeuristic
        {
            get
            {
                //TODO: améliorer
                if (_board.OurPositions().Count.Equals(0))
                {
                    return 0;
                }
                else if (_board.EnnemyPositions().Count.Equals(0))
                {
                    return 1;
                }
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

                if (denominateur.Equals(0))
                {
                    return 0;
                }

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

                if (enDenominateur.Equals(0))
                {
                    return 1;
                }

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
                EnnemyDistance = EnnemyDistance / _board.EnnemyPositions().Count;

                return OurDistance - EnnemyDistance;
            }
        }

        float OurHumanDistance
        {
            get
            {
                Dictionary<Coord, int> humanPositions = this._board.HumanPositions();
                Dictionary<Coord, int> ourPositions = this._board.OurPositions();

                int totalDistance = 0;

                foreach(KeyValuePair<Coord, int> ourPos in ourPositions)
                {
                    int minDistance = int.MaxValue;
                    foreach(KeyValuePair<Coord, int> humPos in humanPositions)
                    {
                        // Si les humains sont plus nombreux on ne les prends pas en compte
                        if (humPos.Value > ourPos.Value)
                        {
                            continue;
                        }

                        // Sinon on regarde la distance
                        int distance = Math.Min(Math.Abs(humPos.Key.X - ourPos.Key.X), Math.Abs(humPos.Key.Y - ourPos.Key.Y));
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }

                    totalDistance += minDistance == int.MaxValue ? 0 : minDistance;
                }

                return (float) -totalDistance;
            }
        }

        public float GetScore()
        {
            float.TryParse(ConfigurationManager.AppSettings["ourNumber"], out float ourNumber);
            float.TryParse(ConfigurationManager.AppSettings["ennemyNumber"], out float ennemyNumber);
            float.TryParse(ConfigurationManager.AppSettings["biggestGroup"], out float biggestGroup);
            float.TryParse(ConfigurationManager.AppSettings["density"], out float density);
            float.TryParse(ConfigurationManager.AppSettings["dispersion"], out float dispersion);
            float.TryParse(ConfigurationManager.AppSettings["ourHumanDistance"], out float ourHumanDistance);

            return (
                ourNumber * this.OurStrengthHeuristic -
                ennemyNumber * this.EnnemyStrengthHeuristic + 
                biggestGroup * this.BiggestGroupHeuristic +
                density * this.DensityHeuristic + 
                dispersion * this.DispersionHeuristic +
                ourHumanDistance * this.OurHumanDistance
            );
        }

    }
}
