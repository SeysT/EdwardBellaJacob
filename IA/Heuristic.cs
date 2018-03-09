/*using IA.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    enum Race { Vampire, Werewolf, Human };

    // class to compute scores
    // To create a new subclass, you only need to touch "ComputeHeuristic()"
    // which is conveniently protected virtual.
    // For all else modifications, ask Hyunwoo
    class Board
    {

    }

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

        protected virtual void StrengthHeuristic()
        {
            h11 = board.grid.Our_number();
        }

        protected virtual void StrengthHeuristic()
        {
            h12 = board.grid.Ennemy_number();
        }

        protected virtual void BiggestGroupHeuristic()
        {
<<<<<<< Updated upstream
            v = board.GetVampire();
            w = board.GetWerewolf();
            this.r = r;
=======
            List<int> OurPositions = new List<int>();
            foreach (Tuple<char, int> t in board.grid.Our_positions().Values())
            {
                OurPositions.Add((t.Item2));
            }

            List<int> EnnemyPositions = new List<int>();
            foreach (Tuple<char, int> t in board.grid.Ennemy_positions().Values())
            {
                EnnemyPositions.Add((t.Item2));
            }

            h2 = OurPositions.Max() - EnnemyPositions.Max();
>>>>>>> Stashed changes
        }

        protected virtual void DensityHeuristic()
        {
            h3 = board.grid.Our_number() / board.grid.Our_positions().count() - board.grid.Ennemy_number() / board.grid.Ennemy_positions().count();
        }

        protected virtual void DispersionHeuristic()
        {
            List<float> OurBarycentre = new List<float>();
            List<float> EnnemyBarycentre = new List<float>();

            int numerateur_x = 0;
            int numerateur_y = 0;
            int denominateur_x = 0;
            int denominateur_y = 0;
            foreach (var element in board.grid.Our_positions())
            {
                numerateur_x += element.Key.Item1 * element.Value.Item2;
                numerateur_y += element.Key.Item2 * element.Value.Item2;
                denominateur_x += element.Key.Item1;
                denominateur_y += element.Key.Item2;
            }
            OurBarycentre[0] = numerateur_x / denominateur_x;
            OurBarycentre[1] = numerateur_y / denominateur_y;

            int en_numerateur_x = 0;
            int en_numerateur_y = 0;
            int en_denominateur_x = 0;
            int en_denominateur_y = 0;
            foreach (var element in board.grid.Ennemy_positions())
            {
                en_numerateur_x += element.Key.Item1 * element.Value.Item2;
                en_numerateur_y += element.Key.Item2 * element.Value.Item2;
                en_denominateur_x += element.Key.Item1;
                en_denominateur_y += element.Key.Item2;
            }
            EnnemyBarycentre[0] = en_numerateur_x / en_denominateur_x;
            EnnemyBarycentre[1] = en_numerateur_y / en_denominateur_y;


            float OurDistance = 0;
            float EnnemyDistance = 0;
            foreach (Tuple<int, int> coord in board.grid.Our_positions().Keys())
            {
                OurDistance += Math.Max(coord.Item1 - OurBarycentre[0], coord.Item2 - OurBarycentre[1]);
            }
            foreach (Tuple<int, int> coord in board.grid.Ennemy_positions().Keys())
            {
                EnnemyDistance += Math.Max(coord.Item1 - EnnemyBarycentre[0], coord.Item2 - EnnemyBarycentre[1]);
            }
            OurDistance = OurDistance / board.grid.Our_positions().count();
            EnnemyDistance = EnnemyDistance / board.grid.Ennemy_positions().count();


            h4 = OurDistance - EnnemyDistance;
        }

        public float Score(float a11, float a12, float a2, float a3, float a4)
        {
            return a11 * h11 + a12 * h12 + a2 * h2 + a3 * h3 + a4 * h4;
        }

    }
}*/
