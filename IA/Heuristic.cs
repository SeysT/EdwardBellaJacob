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
            this.board = board;
        }

        private Board board;
        int h11 = 0;
        int h12 = 0;
        int h2 = 0;
        int h3 = 0;
        float h4 = 0;

        protected virtual void OurStrengthHeuristic()
        {
            h11 = board.grid.OurNumber();
        }

        protected virtual void EnnemyStrengthHeuristic()
        {
            h12 = board.grid.EnnemyNumber();
        }

        protected virtual void BiggestGroupHeuristic()
        {
            List<int> Our_Positions = new List<int>();
            foreach (int value in board.grid.OurPositions())
            {
                Our_Positions.Add((value));
            }

            List<int> Ennemy_Positions = new List<int>();
            foreach (int value in board.grid.EnnemyPositions())
            {
                Ennemy_Positions.Add((value));
            }

            h2 = Our_Positions.Max() - Ennemy_Positions.Max();
        }

        protected virtual void DensityHeuristic()
        {
            h3 = board.grid.OurNumber() / board.grid.OurPositions().count() - board.grid.EnnemyNumber() / board.grid.EnnemyPositions().count();
        }

        protected virtual void DispersionHeuristic()
        {
            List<float> OurBarycentre = new List<float>();
            List<float> EnnemyBarycentre = new List<float>();

            int numerateur_x = 0;
            int numerateur_y = 0;
            int denominateur = board.grid.OurNumber();
            foreach (KeyValuePair<Coord, int> kvp in board.grid.OurPositions())
            {
                numerateur_x += kvp.Key.x * kvp.Value;
                numerateur_y += kvp.Key.y * kvp.Value;
            }
            OurBarycentre[0] = numerateur_x / denominateur;
            OurBarycentre[1] = numerateur_y / denominateur;

            int en_numerateur_x = 0;
            int en_numerateur_y = 0;
            int en_denominateur = board.grid.EnnemyNumber();
            foreach (KeyValuePair<Coord, int> kvp in board.grid.EnnemyPositions())
            {
                en_numerateur_x += kvp.Key.x * kvp.Value;
                en_numerateur_y += kvp.Key.y * kvp.Value;
            }
            EnnemyBarycentre[0] = en_numerateur_x / en_denominateur;
            EnnemyBarycentre[1] = en_numerateur_y / en_denominateur;


            float OurDistance = 0;
            float EnnemyDistance = 0;
            foreach (Coord key in board.grid.OurPositions())
            {
                OurDistance += Math.Max(key.x - OurBarycentre[0], key.y - OurBarycentre[1]);
            }
            foreach (Coord key in board.grid.EnnemyPositions())
            {
                EnnemyDistance += Math.Max(key.x - EnnemyBarycentre[0], key.y - EnnemyBarycentre[1]);
            }
            OurDistance = OurDistance / board.grid.OurPositions().count();
            EnnemyDistance = EnnemyDistance / board.grid.EnnemyPositions().count();


            h4 = OurDistance - EnnemyDistance;
        }

        public float Score(float a11, float a12, float a2, float a3, float a4)
        {
            return a11 * h11 - a12 * h12 + a2 * h2 + a3 * h3 + a4 * h4;
        }

    }
}
