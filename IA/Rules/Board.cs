﻿using IA.Rules;
using System;
using System.Collections.Generic;
using System.Linq;


namespace IA.Rules
{
    public class Board
    // Class to handle boards
    {
        public Grid Grid { get; private set; }
        private int _xMax;
        private int _yMax;
        
        public Board(Board b)
        {
            this.Grid = new Grid(b.Grid);
            this._xMax = b._xMax;
            this._yMax = b._yMax;
        }

        public Board(Grid grid, int xMax, int yMax)
        {
            this.Grid = new Grid(grid);
            this._xMax = xMax;
            this._yMax = yMax;
        }

        public override string ToString()
        {
            string sb = "";
            foreach(Pawn p in Grid.GetPawns())
            {
                sb += $"[{p.Race} = {p.Quantity} : {p.Coordinates.X} , {p.Coordinates.Y} ] ";
            }
            return sb;
        }

        public Board MakeMove(List<Move> moves)
        {
            Board newBoard = new Board(this);
            newBoard.UpdateTable(moves);
            return newBoard;
        }

        public void UpdateTable(List<Move> moves)
        {
            foreach (Move move in moves)
            {
                Pawn pawn = Grid.GetInCoord(move.Coordinates);
                Grid.SetQuantityInCoord(pawn.Coordinates, pawn.Quantity - move.Quantity);

                Coord newCoord = Coord.DirectionMove(pawn.Coordinates, move.Direction);

                Pawn inNewCoord = Grid.GetInCoord(newCoord);
                if (inNewCoord.Quantity == 0)
                {
                    Grid.Add(new Pawn(pawn.Race, move.Quantity, newCoord));
                }
                else
                {
                    if (inNewCoord.Race.Equals(pawn.Race))
                    {
                        Grid.Remove(inNewCoord.Coordinates);
                        Grid.Add(new Pawn(pawn.Race, move.Quantity + inNewCoord.Quantity, newCoord));
                    }
                    else if (inNewCoord.Race.Equals(Race.HUM))
                    {
                        if (inNewCoord.Quantity <=  move.Quantity)
                        {
                            Grid.Remove(inNewCoord.Coordinates);
                            Grid.Add(new Pawn(pawn.Race, move.Quantity + inNewCoord.Quantity, newCoord));
                        }
                        else
                        {
                            //TODO: proba proba proba proba proba proba
                            //on dit pour le moment que si les humains sont plus nombreux, on est mort
                            break;
                        }
                    }
                    else if (!inNewCoord.Race.Equals(pawn.Race))
                    {
                        if (inNewCoord.Quantity >= 1.5 * move.Quantity)
                        {
                            break;
                        }
                        else if(1.5 * inNewCoord.Quantity <= move.Quantity)
                        {
                            Grid.Remove(inNewCoord.Coordinates);
                            Grid.Add(new Pawn(pawn.Race, move.Quantity, newCoord));
                        }
                        else
                        {
                            // TODO: proba proba proba proba
                            if(inNewCoord.Quantity.Equals(move.Quantity))
                            {
                                // Proba de gagner = 0.5
                                // si attaquant gagne, chaque pion a une proba de surivie de P
                                // si attaquant perd , chaque pion a une proba de survie 1 - P
                                Grid.Remove(inNewCoord.Coordinates);
                                Grid.Add(new Pawn(pawn.Race, (int)(move.Quantity * 0.5) , newCoord));

                            }
                            else if (inNewCoord.Quantity > move.Quantity)
                            {
                                // Proba de gagner = move.Quantity / (2 * inNewCoord.Quantity)
                                Grid.Remove(inNewCoord.Coordinates);
                                Grid.Add(new Pawn(inNewCoord.Race, (int)(inNewCoord.Quantity * (1 - move.Quantity /(2 * inNewCoord.Quantity))), inNewCoord.Coordinates));
                            }
                            else if (inNewCoord.Quantity < move.Quantity)
                            {
                                // Proba de gagner = move.Quantity / inNewCoord.Quantity - 0.5
                                Grid.Remove(inNewCoord.Coordinates);
                                Grid.Add(new Pawn(pawn.Race, (int)(move.Quantity * (move.Quantity / inNewCoord.Quantity - 0.5)), newCoord));
                            }
                        }
                    }   
                }
            }
        }

        /// <summary>
        /// Get the min number of a group on the board
        /// </summary>
        public int GetMinGroupNumber()
        {
            int ourNumber = this.OurNumber();
            int min = ourNumber;
            foreach (Pawn pawn in this.Grid.GetPawns())
            {
                if (pawn.Quantity < min)
                {
                    min = pawn.Quantity;
                }
            }
            return Math.Max(min, (int)(ourNumber / 3));
        }

        public List<List<Move>> GetPossibleMoves(Race race, int maxSplitGroups, Boolean split)
        {
            List<List<Move>> outList = new List<List<Move>>();
            // List<Move> represents one possible move combination that can be played during a turn
            // List<List<Move>> represents all possible move combinations.

            var sequenceList = new List<List<List<Move>>>();
            // in sequenceList :
            // List<Move> = one move for one Pawn.
            // It's a List because if there's a split, it needs to be a list.
            // List<List<Move>> = represents all possible moves for a pawn
            // third List layer is for different pawns

            List<Pawn> pawns = race == Race.US ? this.OurPawns() : this.EnnemyPawns();
            int minSplitValue = split ? this.GetMinGroupNumber() : this.OurNumber();

            foreach (Pawn pawn in pawns)
            {
                sequenceList.Add(GetAllConfigurationForOnePawn(pawn, minSplitValue, maxSplitGroups));
            }
            var sequenceArray = sequenceList.ToArray();
            
            foreach (var array in sequenceList.Permutations(a => a))
            {
                outList.Add(array.Aggregate(new List<Move>(), (list, i) => { list.AddRange(i); return list; }));
            }
            return outList;
        }

        public List<List<Move>> GetAllConfigurationForOnePawn(Pawn pawn, int minSplit, int maxSplitGroups)
        {
            List<List<Move>> allConfs = new List<List<Move>>();
            List<Direction> possibleDirections = this.GetPossibleDirections(pawn.Coordinates);
            foreach(int[] configuration in SplitEnumeration.GetEnumeration(pawn.Quantity, minSplit, maxSplitGroups))
            {
                List<Move> allMoves = new List<Move>();
                for(int i =0; i < 8; i++) 
                {
                    if (possibleDirections.Contains((Direction)i) && configuration[i] > 0)
                    {
                        allMoves.Add(new Move(pawn.Coordinates, (Direction)i, configuration[i]));
                    }
                }

                if(allMoves.Count >0)
                    allConfs.Add(allMoves);
            }
            return allConfs;
        }


        /// <summary>
        /// Get all possible coord and direction where we can move
        /// </summary>
        /// <param name="coordToCheck">Coord from which we want to calculate possible move in all directions</param>
        /// <returns>use keys to get possible coords and values to get possible directions</returns>
        public List<Direction> GetPossibleDirections(Coord coordToCheck)
        {
            Dictionary<Coord, Direction> possibleCoordDirections = Coord.PossibleCoordsFromDirectionMoves(coordToCheck);
            List<Direction> coordDirections = new List<Direction>();
            foreach (Coord coord in possibleCoordDirections.Keys)
            {
                if (coord.X >= 0
                    && coord.X < _xMax
                    && coord.Y >= 0
                    && coord.Y < _yMax)
                {
                    coordDirections.Add(possibleCoordDirections[coord]);
                }
            }
            return coordDirections;
        }

        /// <summary>
        /// Get race list pawns from given race.
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public List<Pawn> RacePawns(Race race)
        {
            List<Pawn> list = new List<Pawn>();

            foreach (Pawn pawn in Grid.GetPawns())
            {
                if (pawn.Race.Equals(race))
                {
                    list.Add(pawn);
                }
            }
            return list;
        }

        public List<Pawn> OurPawns()
        {
            return this.RacePawns(Race.US);
        }

        public List<Pawn> EnnemyPawns()
        {
            return this.RacePawns(Race.THEM);
        }

        public List<Pawn> HumanPawns()
        {
            return this.RacePawns(Race.HUM);
        }

        /// <summary>
        /// Get race positions on grid from given race
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public Dictionary<Coord, int> RacePositions(Race race)
        {
            Dictionary<Coord, int> dict = new Dictionary<Coord, int>();

            foreach (Pawn pawn in Grid.GetPawns())
            {
                if (pawn.Race.Equals(race))
                {
                    dict.Add(pawn.Coordinates, pawn.Quantity);
                }
            }
            return dict;
        }

        public Dictionary<Coord, int> OurPositions()
        {
            return this.RacePositions(Race.US);
        }

        public Dictionary<Coord, int> EnnemyPositions()
        {
            return this.RacePositions(Race.THEM);
        }

        public Dictionary<Coord, int> HumanPositions()
        {
            return this.RacePositions(Race.HUM);
        }

        /// <summary>
        /// Get race number from given race
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public int RaceNumber(Race race)
        {
            int number = 0;
            foreach (Pawn pawn in Grid.GetPawns())
            {
                if (pawn.Race.Equals(race))
                {
                    number += pawn.Quantity;
                }
            }
            return number;
        }

        public int OurNumber()
        {
            return this.RaceNumber(Race.US);
        }

        public int EnnemyNumber()
        {
            return this.RaceNumber(Race.THEM);
        }

        public int HumanNumber()
        {
            return this.RaceNumber(Race.HUM);
        }
    }
}


public static class EnumerableExtensions
{
    public static IEnumerable<TValue[]> Permutations<TKey, TValue>(this IEnumerable<TKey> keys, Func<TKey, IEnumerable<TValue>> selector)
    {
        var keyArray = keys.ToArray();
        if (keyArray.Length < 1)
            yield break;
        TValue[] values = new TValue[keyArray.Length];
        foreach (var array in Permutations(keyArray, 0, selector, values))
            yield return array;
    }

    static IEnumerable<TValue[]> Permutations<TKey, TValue>(TKey[] keys, int index, Func<TKey, IEnumerable<TValue>> selector, TValue[] values)
    {
        var key = keys[index];
        foreach (var value in selector(key))
        {
            values[index] = value;
            if (index < keys.Length - 1)
            {
                foreach (var array in Permutations(keys, index + 1, selector, values))
                    yield return array;
            }
            else
            {
                yield return values.ToArray(); // Clone the array;
            }
        }
    }
}

