using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    class MinMax
    {
        
        // Alpha Beta is a branch and bound variation of min max algorithm
        // It allowed us to jump from a 2-depth to a 5-depth and thus to make AI stronger

        // Scores are considered are from the opponent's point of view.
        // Positive scores is losing
        // Negative scores is winning

        // TODO add Move class
        // TODO add to Board "MakeMove(currentMove)" method to update board

        private float AlphaBeta(Node current, 
                              int depth, 
                              float alpha, 
                              float beta, 
                              Race myRace,
                              bool isMyTurn,
                              List<Move> movesCandidate, 
                              Board board)
        {
            if (movesCandidate.Count() == 0 || depth == 0)
            {
                return current.Data.HeuristicScore;
            }
            else
            {



                if (isMyTurn)
                {
                    float val = (float) Int32.MaxValue;
                    foreach (Move currentMove in movesCandidate)
                    {
                        // Compute new board after move
                        // TODO implement Board.MakeMove
                        Board newBoard = new Board(board).MakeMove(currentMove);

                        // fetch the new movesCandidate.
                        // TODO implement Board.getMoves(Race r)
                        List<Move> updatedMoveCandidate = new List<Move>(movesCandidate);
                        updatedMoveCandidate.Remove(currentMove);
                        
                        // The new Leaf is the child we're going to explore next
                        Node Child = new Node(new NodeData());

                        // Add each child Tree to parent node
                        current.Children.Add(Child);

                        // Compute Node.Weight
                        // TODO implement Board.getHeuristicScore(Race)
                        Child.Data.HeuristicScore = newBoard.getHeuristicScore(myRace);

                        //Alpha beta stuff
                        val = Math.Max(val, this.AlphaBeta(Child, depth - 1, alpha, beta, myRace, !isMyTurn, updatedMoveCandidate, newBoard));
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
                    float val = (float)Int32.MaxValue;
                    foreach (Move currentMove in movesCandidate)
                    {

                        // Compute new board after move
                        // TODO implement Board.MakeMove
                        Board newBoard = new Board(board).MakeMove(currentMove);

                        // fetch the new movesCandidate.
                        // TODO implement Board.getMoves(Race r)
                        List<Move> updatedMoveCandidate = new List<Move>(movesCandidate);
                        updatedMoveCandidate.Remove(currentMove);

                        // The new Leaf is the child we're going to explore next
                        Node Child = new Node(new NodeData());

                        // Add each child Tree to parent node
                        current.Children.Add(Child);

                        // Compute Node.Weight
                        // TODO implement Board.getHeuristicScore(Race)
                        Race otherRace = myRace == Race.Vampire ? Race.Werewolf : Race.Vampire;
                        Child.Data.HeuristicScore = newBoard.getHeuristicScore(otherRace);

                        //Alpha beta stuff
                        val = Math.Min(val, this.AlphaBeta(Child, depth - 1, alpha, beta, myRace, !isMyTurn, updatedMoveCandidate, newBoard));
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
        }

        // This function alows to convert the score returned by AlphaBeta to the coordinates of the next currentMove
        private static Move GetNextMove(Node GameTree, int bestScore)
        {
            foreach (Node SubTree in GameTree.Children)
            {
                if (SubTree.Data.MinMaxScore == bestScore)
                {
                    return SubTree.Data.Moves;
                }
            }
            throw new Exception("[MinMax] GetNextMove couldn't return LastPlayedPosition");
        }
    }
}
