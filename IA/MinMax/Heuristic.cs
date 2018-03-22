using IA.Rules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    // class to compute scores
    // For all modifications, ask Anouar and Baudouin

    public class Heuristic
    {
        private static Heuristic _instance;
        private static readonly object _lock = new object();

        private float ourNumber;
        private float ennemyNumber;
        private float biggestGroup;
        private float density;
        private float dispersion;
        private float ourHumanDistance;

        public Heuristic()
        {
            NumberStyles style = NumberStyles.AllowDecimalPoint;
            CultureInfo culture = CultureInfo.InvariantCulture;

            float.TryParse(ConfigurationManager.AppSettings["ourNumber"], style, culture, out ourNumber);
            Trace.TraceInformation($"OurNumber coefficient : {ourNumber}");
            float.TryParse(ConfigurationManager.AppSettings["ennemyNumber"], style, culture, out ennemyNumber);
            Trace.TraceInformation($"EnnemyNumber coefficient : {ennemyNumber}");
            float.TryParse(ConfigurationManager.AppSettings["biggestGroup"], style, culture, out biggestGroup);
            Trace.TraceInformation($"BiggestGroup coefficient : {biggestGroup}");
            float.TryParse(ConfigurationManager.AppSettings["density"], style, culture, out density);
            Trace.TraceInformation($"Density coefficient : {density}");
            float.TryParse(ConfigurationManager.AppSettings["dispersion"], style, culture, out dispersion);
            Trace.TraceInformation($"Dispersion coefficient : {dispersion}");
            float.TryParse(ConfigurationManager.AppSettings["ourHumanDistance"], style, culture, out ourHumanDistance);
            Trace.TraceInformation($"OurHumanDistance coefficient : {ourHumanDistance}");
        }

        public static Heuristic Instance
        {
            get
            {
                if (_instance == null) // Les locks prennent du temps, il est préférable de vérifier d'abord la nullité de l'instance.
                {
                    lock (_lock)
                    {
                        if (_instance == null) // on vérifie encore, au cas où l'instance aurait été créée entretemps.
                            _instance = new Heuristic();
                    }
                }
                return _instance;
            }
        }

        public float GetOurStrengthHeuristic(Board board)
        {
            return board.OurNumber();
        }

        public float GetEnnemyStrengthHeuristic(Board board)
        {
            return board.EnnemyNumber();
        }

        public float GetBiggestGroupHeuristic(Board board)
        { 
            List<int> OurPositions = new List<int>();
            foreach (KeyValuePair<Coord, int> keyPair in board.OurPositions())
                OurPositions.Add((keyPair.Value));

            List<int> EnnemyPositions = new List<int>();
            foreach (KeyValuePair<Coord, int> keyPair in board.EnnemyPositions())
                EnnemyPositions.Add((keyPair.Value));

            if (OurPositions.Count.Equals(0) || EnnemyPositions.Count.Equals(0))
                if (OurPositions.Count.Equals(0) && EnnemyPositions.Count.Equals(0))
                    return 0;
                else if (OurPositions.Count.Equals(0))
                    return - EnnemyPositions.Max();
                else
                    return OurPositions.Max();
            return OurPositions.Max() - EnnemyPositions.Max();
        }

        public float GetDensityHeuristic(Board board)
        {
            int ourNumber = board.OurNumber();
            int ennemyNumber = board.EnnemyNumber();

            if (ourNumber == 0 || ennemyNumber == 0)
                if (ourNumber == 0 && ennemyNumber == 0)
                    return 0;
                else if (ourNumber == 0)
                    return -ennemyNumber / board.EnnemyPositions().Count;
                else
                    return ourNumber / board.OurPositions().Count;
            return ourNumber / board.OurPositions().Count - ennemyNumber / board.EnnemyPositions().Count;
        }

        public float GetDispersionHeuristic(Board board)
        {
            float[] OurBarycentre = new float[2];
            float[] EnnemyBarycentre = new float[2];

            int numerateurX = 0;
            int numerateurY = 0;
            int denominateur = board.OurNumber();

            float OurDistance = 0;

            if (!denominateur.Equals(0))
            {
                foreach (KeyValuePair<Coord, int> kvp in board.OurPositions())
                {
                    numerateurX += kvp.Key.X * kvp.Value;
                    numerateurY += kvp.Key.Y * kvp.Value;
                }
                OurBarycentre[0] = numerateurX / denominateur;
                OurBarycentre[1] = numerateurY / denominateur;

                foreach (KeyValuePair<Coord, int> keyPair in board.OurPositions())
                {
                    OurDistance += Math.Max(keyPair.Key.X - OurBarycentre[0], keyPair.Key.Y - OurBarycentre[1]);
                }
                OurDistance = OurDistance / board.OurPositions().Count;
            }

            int enNumerateurX = 0;
            int enNumerateurY = 0;
            int enDenominateur = board.EnnemyNumber();

            float EnnemyDistance = 0;

            if (!enDenominateur.Equals(0))
            {
                foreach (KeyValuePair<Coord, int> kvp in board.EnnemyPositions())
                {
                    enNumerateurX += kvp.Key.X * kvp.Value;
                    enNumerateurY += kvp.Key.Y * kvp.Value;
                }
                EnnemyBarycentre[0] = enNumerateurX / enDenominateur;
                EnnemyBarycentre[1] = enNumerateurY / enDenominateur;

                foreach (KeyValuePair<Coord, int> keyPair in board.EnnemyPositions())
                {
                    EnnemyDistance += Math.Max(keyPair.Key.X - EnnemyBarycentre[0], keyPair.Key.Y - EnnemyBarycentre[1]);
                }
            
                EnnemyDistance = EnnemyDistance / board.EnnemyPositions().Count;
            }
            return OurDistance - EnnemyDistance;
        }

        public float GetOurHumanDistance(Board board)
        {
            Dictionary<Coord, int> humanPositions = board.HumanPositions();
            Dictionary<Coord, int> ourPositions = board.OurPositions();

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

            return (float) totalDistance;
        }

        public float GetScore(Board board)
        {
            return (
                ourNumber * this.GetOurStrengthHeuristic(board) -
                ennemyNumber * this.GetEnnemyStrengthHeuristic(board) + 
                biggestGroup * this.GetBiggestGroupHeuristic(board) +
                density * this.GetDensityHeuristic(board) + 
                dispersion * this.GetDispersionHeuristic(board) -
                ourHumanDistance * this.GetOurHumanDistance(board)
            );
        }
    }
}
