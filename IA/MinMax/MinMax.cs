﻿using IA.IA;
using IA.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    public class MinMax: BaseIA
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
        private int _depth;
        private float _maxAlpha;
        private float _minBeta;

        public MinMax(int depth): base()
        {
            this._depth = depth;
            this._maxAlpha = float.MinValue;
            this._minBeta = float.MaxValue;
        }

        private float _getAlphaBeta(Node current, int depth, float alpha, float beta, bool isMyTurn, Board board)
        {
            
            if (depth == 0)
            {
                current.Data.MinMaxScore = current.Data.HeuristicScore;
                return current.Data.HeuristicScore;
            }

            if (isMyTurn)
            {
                List<Move> movesCandidate = board.GetPossibleMoves(Race.US);
                float val = alpha;
                //à checker
                if (movesCandidate.Count.Equals(0))
                {
                    
                    current.Data.HeuristicScore = this._getHeuristicScore(board);

                    //Alpha beta stuff
                    val = current.Data.HeuristicScore;
                    if (val >= beta)
                    {
                        return val;
                    }
                    else
                    {
                        alpha = Math.Max(alpha, val);
                    }
                }
                foreach (Move currentMove in movesCandidate)
                {
                    // Compute new board after move
                    Board newBoard = board.MakeMove(new List<Move>() { currentMove });
                        
                    // The new Leaf is the child we're going to explore next
                    Node Child = new Node();

                    // Add each child Tree to parent node
                    current.Children.Add(Child);
                    Child.Data.MinMaxScore = alpha;
                    // Compute Node.Weight

                    Child.Data.HeuristicScore = this._getHeuristicScore(newBoard);
                    Child.Data.Moves = new List<Move>() { currentMove };

                    //Alpha beta stuff
                    val = Math.Max(val, this._getAlphaBeta(Child, depth - 1, alpha, beta, !isMyTurn, newBoard));
                    if (val >= beta)
                    {
                        break;
                    }
                    else
                    {
                        alpha = Math.Max(alpha, val);
                    }
                }
                current.Data.MinMaxScore =Math.Min(current.Data.MinMaxScore,val);
                return val;
            }
            else
            {
                float val = beta;
                List<Move> movesCandidate = board.GetPossibleMoves(Race.THEM);
                //if movescandidate.count.equals(0)
                foreach (Move currentMove in movesCandidate)
                {
                    // Compute new board after move
                    Board newBoard = board.MakeMove(new List<Move>() { currentMove });

                    // The new Leaf is the child we're going to explore next
                    Node Child = new Node();

                    // Add each child Tree to parent node
                    current.Children.Add(Child);
                    Child.Data.MinMaxScore = beta;
                    // Compute Node.Weight
                    Child.Data.HeuristicScore = this._getHeuristicScore(newBoard);
                    Child.Data.Moves = new List<Move>() { currentMove };

                    //Alpha beta stuff
                    //val = Child.Data.HeuristicScore;
                    val = Math.Min(val, this._getAlphaBeta(Child, depth - 1, alpha, beta, !isMyTurn, newBoard));
                    
                    if (val < alpha)
                    {
                        break;
                    }
                    else
                    {
                        beta = Math.Min(beta, val);
                    }
                }
                if(val <= _maxAlpha)
                {
                    System.Console.WriteLine("Problem");
                }
                current.Data.MinMaxScore = Math.Max(val, current.Data.MinMaxScore);
                return val;
            }
        }

        // This function alows to convert the score returned by AlphaBeta to the coordinates of the next currentMove
        private List<Move> _getNextMove(Node GameTree, float alphaBeta)
        {
            foreach (Node SubTree in GameTree.Children)
            {
                if (SubTree.Data.MinMaxScore == alphaBeta)
                {
                    return SubTree.Data.Moves;
                }
            }
            throw new Exception("[MinMax] GetNextMove couldn't return Moves");
        }

        private float _getHeuristicScore(Board board)
        {
            return new Heuristic(board).GetScore();
        }

        override public int[,] ChooseNextMove(Board board)
        {
            Node root = new Node();
            float alphaBeta = this._getAlphaBeta(root, this._depth, this._maxAlpha, this._minBeta, true, board);
            return MOVTrame.GetPayloadFromMoves(this._getNextMove(root, alphaBeta));
        }
    }
}
