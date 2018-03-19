using System.Collections.Generic;

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

                // No pawn in new coordinates, we can move in
                if (inNewCoord.Quantity == 0)
                {
                    Grid.Add(new Pawn(pawn.Race, move.Quantity, newCoord));
                }
                else
                {
                    // Pawn in new coordinates get same race has the one we want to move --> we merge
                    if (inNewCoord.Race.Equals(pawn.Race))
                    {
                        Grid.Remove(inNewCoord.Coordinates);
                        Grid.Add(new Pawn(pawn.Race, move.Quantity + inNewCoord.Quantity, newCoord));
                    }
                    // Pawn in new coordinates are humans --> we fight
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
                    // Pawn in new coordinates are two enemy races --> fight !
                    else
                    {
                        if (inNewCoord.Quantity >= 1.5 * move.Quantity)
                        {
                            break;
                        }
                        else if(1.5 * inNewCoord.Quantity <= move.Quantity)
                        {
                            Grid.Remove(inNewCoord.Coordinates);
                            Grid.Add(new Pawn(pawn.Race, move.Quantity + inNewCoord.Quantity, newCoord));
                        }
                        else
                        {
                            // TODO: proba proba proba proba
                            if(inNewCoord.Quantity.Equals(move.Quantity))
                            {
                                // Proba de gagner = 0.5
                                // si attaquant gagne, chaque pion a une proba de surivie de P
                                // si attaquant perd , chaque pion a une proba de survie 1 - P
                                Grid.Remove(inNewCoord);
                                Grid.Add(new Pawn(pawn.Race, (int)(move.Quantity * 0.5) , newCoord));

                            }
                            else if (inNewCoord.Quantity > move.Quantity)
                            {
                                // Proba de gagner = move.Quantity / (2 * inNewCoord.Quantity)
                                Grid.Remove(inNewCoord);
                                Grid.Add(new Pawn(inNewCoord.Race, (int)(inNewCoord.Quantity * (1 - move.Quantity /(2 * inNewCoord.Quantity))), inNewCoord.Coordinates));
                            }
                            else
                            {
                                // Proba de gagner = move.Quantity / inNewCoord.Quantity - 0.5
                                Grid.Remove(inNewCoord);
                                Grid.Add(new Pawn(pawn.Race, (int)(move.Quantity * (move.Quantity / inNewCoord.Quantity - 0.5)), newCoord));
                            }
                        }
                    }   
                }
            }
        }

        public List<Move> GetPossibleMoves(Race race)
        {
            //TO MODIFY: Take into account Split moves
            List<Move> list = new List<Move>();
            List<Pawn> pawns = race == Race.US ? this.OurPawns() : this.EnnemyPawns();
            foreach (Pawn pawn in pawns)
            {
                Dictionary<Coord, Direction> possibleDirections = this.GetPossibleCoordDirections(pawn.Coordinates);
                foreach (Direction direction in possibleDirections.Values)
                {
                    list.Add(new Move(pawn.Coordinates, direction, pawn.Quantity));
                }
            }
            return list;
        }

        /// <summary>
        /// Get all possible coord and direction where we can move
        /// </summary>
        /// <param name="coordToCheck">Coord from which we want to calculate possible move in all directions</param>
        /// <returns>use keys to get possible coords and values to get possible directions</returns>
        public Dictionary<Coord, Direction> GetPossibleCoordDirections(Coord coordToCheck)
        {
            Dictionary<Coord, Direction> possibleCoordDirections = Coord.PossibleCoordsFromDirectionMoves(coordToCheck);
            Dictionary<Coord, Direction> coordDirections = new Dictionary<Coord, Direction>();
            foreach (Coord coord in possibleCoordDirections.Keys)
            {
                if (coord.X >= 0
                    && coord.X < _xMax
                    && coord.Y >= 0
                    && coord.Y < _yMax)
                {
                    coordDirections.Add(coord, possibleCoordDirections[coord]);
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
                if (pawn.Race.Equals(Race.US))
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