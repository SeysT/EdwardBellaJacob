using IA.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.IA
{
    public abstract class BaseIA
    {
        public float score;
        public bool AlphaBetaFinished;

        public BaseIA()
        {
            this.AlphaBetaFinished = false;
        }

        public abstract void ComputeNextMove(Board board);
        public abstract int[,] ChooseNextMove();
    }
}
