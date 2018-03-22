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
        private Stopwatch _sw;

        public BaseIA()
        {
            this._sw = new Stopwatch();
        }

        public float score;
        public bool AlphaBetaFinished;
        public abstract void ComputeNextMove(Board board);
        public abstract int[,] ChooseNextMove();
    }
}
