using IA.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    class MinMax
    {
        // Alpha Beta is a branch and bound variation of min max algorithm
        // It allowed us to jump from a 2-depth to a 5-depth and thus to make AI stronger

        // Scores are considered are from the opponent's point of view.
        // Positive scores is losing
        // Negative scores is winning

        // TODO add Move class
        // TODO implement Board.MakeMove(Move)
        // TODO implement Board.getMoves(Race r)
        // TODO implement Board.getHeuristicScore(Race)

        public float AlphaBeta(Node current, int depth, float alpha, float beta, bool isMyTurn, List<Move> movesCandidate, Board board)
        {
            if (movesCandidate.Count() == 0 || depth == 0)
            {
                return current.Data.HeuristicScore;
            }

            if (isMyTurn)
            {
                float val = float.MinValue;
                foreach (Move currentMove in movesCandidate)
                {
                    // Compute new board after move
                    Board newBoard = board.MakeMove(new List<Move>() { currentMove });

                    // fetch the new movesCandidate.
                    List<Move> updatedMoveCandidate = new List<Move>(movesCandidate);
                    updatedMoveCandidate.Remove(currentMove);
                        
                    // The new Leaf is the child we're going to explore next
                    Node Child = new Node(new NodeData());

                    // Add each child Tree to parent node
                    current.Children.Add(Child);

                    // Compute Node.Weight
                    Child.Data.HeuristicScore = newBoard.GetHeuristicScore(Type.US);

                    //Alpha beta stuff
                    val = Math.Max(val, this.AlphaBeta(Child, depth - 1, alpha, beta, !isMyTurn, updatedMoveCandidate, newBoard));
                    Child.Data.MinMaxScore = val;
                        
                    beta = Math.Max(beta, val);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return val;
            }
            else
            {
                float val = float.MaxValue;
                foreach (Move currentMove in movesCandidate)
                {
                    // Compute new board after move
                    Board newBoard = board.MakeMove(new List<Move>() { currentMove });

                    // fetch the new movesCandidate.
                    List<Move> updatedMoveCandidate = new List<Move>(movesCandidate);
                    updatedMoveCandidate.Remove(currentMove);

                    // The new Leaf is the child we're going to explore next
                    Node Child = new Node(new NodeData());

                    // Add each child Tree to parent node
                    current.Children.Add(Child);

                    // Compute Node.Weight
                    Child.Data.HeuristicScore = newBoard.GetHeuristicScore(Type.THEM);

                    //Alpha beta stuff
                    val = Math.Min(val, this.AlphaBeta(Child, depth - 1, alpha, beta, !isMyTurn, updatedMoveCandidate, newBoard));
                    Child.Data.MinMaxScore = val;

                    alpha = Math.Min(alpha, val);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return val;
            }
        }

        // This function alows to convert the score returned by AlphaBeta to the coordinates of the next currentMove
        static public List<Move> GetNextMove(Node GameTree, float bestScore)
        {
            foreach (Node SubTree in GameTree.Children)
            {
                if (SubTree.Data.MinMaxScore == bestScore)
                {
                    return SubTree.Data.Moves;
                }
            }
            throw new Exception("[MinMax] GetNextMove couldn't return Moves");
        }

        static public int[,] GetNextMoves()
        {
            throw new NotImplementedException();
        }
    }
}
