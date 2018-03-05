using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*namespace IA
{
    enum Race {Vampire, Werewolf, Human};

    // class to compute scores
    // To create a new subclass, you only need to touch "ComputeHeuristic()"
    // which is conveniently protected virtual.
    // For all else modifications, ask Hyunwoo

    class Heuristic
    {
        public Heuristic(Board board, Race r)
        {
            v = board.getVampire();
            w = board.getWerewolf();
            this.r = r;
        }

        private string Method = "population";   // name of heuristic
        private int Score = 0;
        private int v = 0;                      // vampire population
        private int w = 0;                      // werewolf population
        private Race r = Race.Human;            // If left "human", raise Exception.

        protected virtual void ComputeHeuristic()
        {
            Score = r == Race.Vampire ? v - w : w - v;
        }

        public float Compute()
        {
            ComputeHeuristic();
            return Score;
        }
    }
}*/
