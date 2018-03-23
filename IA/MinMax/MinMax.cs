using IA.IA;
using IA.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    public class MinMax : BaseIA
    {
        // Alpha Beta is a branch and bound variation of min max algorithm
        // It allowed us to jump from a 2-depth to a 5-depth and thus to make AI stronger
        
        private int _depth;

        private float _maxAlpha;
        private float _minBeta;
        private bool _doesSplit;

        private Node _root;
        private float _alphaBeta;

        public MinMax(int depth, bool doesSplit) : base()
        {
            this._depth = depth;
            this._maxAlpha = float.MinValue;
            this._minBeta = float.MaxValue;
            this._doesSplit = doesSplit;

            this._root = new Node();
            this._alphaBeta = float.MinValue;
        }

        private float _getAlphaBeta(Node current, int depth, float alpha, float beta, bool isMyTurn, Board board, Boolean split)
        {
            int maxSplitGroups = split ? 2 : 1;

            if (depth == 0 || board.OurNumber() == 0 || board.EnnemyNumber() == 0)
            {
                current.Data.MinMaxScore = current.Data.HeuristicScore;
                return current.Data.HeuristicScore;
            }

            if (isMyTurn)
            {
                List<List<Move>> movesCandidate = board.GetPossibleMoves(Race.US, maxSplitGroups , split);
                float val = alpha;
                
                if (movesCandidate.Count.Equals(0))
                {
                    current.Data.HeuristicScore = this._getHeuristicScore(board);

                    //Alpha beta stuff
                    val = current.Data.HeuristicScore;
                    if (val >= beta)
                    {
                        return val;
                    }
                    alpha = Math.Max(alpha, val);

                }
                foreach (List<Move> currentMove in movesCandidate)
                {
                    // Compute new board after move
                    Board newBoard = board.MakeMove(new List<Move>(currentMove));

                    // The new Leaf is the child we're going to explore next
                    Node Child = new Node();

                    // Add each child Tree to parent node
                    current.Children.Add(Child);
                    Child.Data.MinMaxScore = alpha;
                    // Compute Node.Weight

                    Child.Data.HeuristicScore = this._getHeuristicScore(newBoard);
                    Child.Data.Moves = new List<Move>(currentMove);

                    //Alpha beta stuff
                    val = Math.Max(val, this._getAlphaBeta(Child, depth - 1, alpha, beta, !isMyTurn, newBoard, split));
                    if (val >= beta)
                    {
                        break;
                    }
                    alpha = Math.Max(alpha, val);
                }
                current.Data.MinMaxScore = val;
                return val;
            }
            else
            {
                float val = beta;
                List<List<Move>> movesCandidate = board.GetPossibleMoves(Race.THEM, maxSplitGroups, split);
                //if movescandidate.count.equals(0)
                foreach (List<Move> currentMove in movesCandidate)
                {
                    // Compute new board after move
                    Board newBoard = board.MakeMove(new List<Move>(currentMove));

                    // The new Leaf is the child we're going to explore next
                    Node Child = new Node();

                    // Add each child Tree to parent node
                    current.Children.Add(Child);
                    Child.Data.MinMaxScore = beta;
                    // Compute Node.Weight
                    Child.Data.HeuristicScore = this._getHeuristicScore(newBoard);
                    Child.Data.Moves = new List<Move>(currentMove);

                    //Alpha beta stuff
                    val = Math.Min(val, this._getAlphaBeta(Child, depth - 1, alpha, beta, !isMyTurn, newBoard, split));

                    if (val <= alpha)
                    {
                        break;
                    }
                    beta = Math.Min(beta, val);
                }
                current.Data.MinMaxScore = val;
                return val;
            }
        }

        // This function alows to convert the score returned by AlphaBeta to the coordinates of the next currentMove
        private List<Move> _getNextMove(Node GameTree, float alphaBeta)
        {
            List<Move> moves = new List<Move>();
            float maxHeuristic = _maxAlpha;
            foreach (Node SubTree in GameTree.Children)
            {
                if (SubTree.Data.MinMaxScore == alphaBeta)
                {
                    //return SubTree.Data.Moves;
                    if (maxHeuristic < SubTree.Data.HeuristicScore)
                    {
                        maxHeuristic = SubTree.Data.HeuristicScore;
                        score = alphaBeta;
                        moves = SubTree.Data.Moves;
                    }
                }
            }
            if (!moves.Count.Equals(0))
            {
                return moves;
            }
            throw new Exception("[MinMax] GetNextMove couldn't return Moves");
        }

        private List<Move> _getNextMove(Node GameTree)
        {
            List<Move> moves = new List<Move>();
            float maxHeuristic = float.NegativeInfinity;
            float maxAlphaBeta = float.NegativeInfinity;
            foreach (Node SubTree in GameTree.Children)
            {
                if (SubTree.Data.MinMaxScore < maxAlphaBeta)
                {
                    //return SubTree.Data.Moves;
                    if (maxHeuristic < SubTree.Data.HeuristicScore)
                    {
                        maxHeuristic = SubTree.Data.HeuristicScore;
                        score = maxAlphaBeta;
                        moves = SubTree.Data.Moves;
                    }
                }
            }
            if (!moves.Count.Equals(0))
            {
                return moves;
            }
            throw new Exception("[MinMax] GetNextMove couldn't return Moves");
        }

        private float _getHeuristicScore(Board board)
        {
            return Heuristic.Instance.GetScore(board);
        }

        override public void ComputeNextMove(Board board)
        {
            _alphaBeta = this._getAlphaBeta(_root, this._depth, this._maxAlpha, this._minBeta, true, board, _doesSplit);
            AlphaBetaFinished = true;
            Trace.TraceInformation("MinMax : " + board.ToString());
        }

        override public int[,] ChooseNextMove()
        {
            if (AlphaBetaFinished)
            {
                return MOVTrame.GetPayloadFromMoves(this._getNextMove(_root, _alphaBeta));
            }
            else
            {
                return MOVTrame.GetPayloadFromMoves(this._getNextMove(_root));
            }
        }
    }
}
