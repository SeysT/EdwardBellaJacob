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
                if (inNewCoord.Quantity == 0)
                {
                    Grid.Pawns.Add(new Pawn(pawn.Race, move.Quantity, newCoord));
                }
                else
                {
                    if (inNewCoord.Race.Equals(pawn.Race))
                    {
                        Grid.Pawns.Remove(inNewCoord);
                        Grid.Pawns.Add(
                            new Pawn(pawn.Race, move.Quantity + inNewCoord.Quantity, newCoord)
                        );
                    }
                    else if (inNewCoord.Race.Equals(Race.HUM))
                    {
                        if (inNewCoord.Quantity <=  move.Quantity)
                        {
                            Grid.Pawns.Remove(inNewCoord);
                            Grid.Pawns.Add(
                                new Pawn(pawn.Race, move.Quantity + inNewCoord.Quantity, newCoord)
                            );
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
                            Grid.Pawns.Remove(inNewCoord);
                            Grid.Pawns.Add(new Pawn(pawn.Race, move.Quantity + inNewCoord.Quantity, newCoord));
                        }
                        else
                        {
                            // TODO: proba proba proba proba
                            if(inNewCoord.Quantity.Equals(move.Quantity))
                            {
                                // Proba de gagner = 0.5
                                // si attaquant gagne, chaque pion a une proba de surivie de P
                                // si attaquant perd , chaque pion a une proba de survie 1 - P
                                Grid.Pawns.Remove(inNewCoord);
                                Grid.Pawns.Add(new Pawn(pawn.Race, (int)(move.Quantity * 0.5) , newCoord));

                            }
                            else if (inNewCoord.Quantity > move.Quantity)
                            {
                                // Proba de gagner = move.Quantity / (2 * inNewCoord.Quantity)
                                Grid.Pawns.Remove(inNewCoord);
                                Grid.Pawns.Add(new Pawn(inNewCoord.Race, (int)(inNewCoord.Quantity * (1 - move.Quantity /(2 * inNewCoord.Quantity))), inNewCoord.Coordinates));
                            }
                            else if (inNewCoord.Quantity < move.Quantity)
                            {
                                // Proba de gagner = move.Quantity / inNewCoord.Quantity - 0.5
                                Grid.Pawns.Remove(inNewCoord);
                                Grid.Pawns.Add(new Pawn(pawn.Race, (int)(move.Quantity * (move.Quantity / inNewCoord.Quantity - 0.5)), newCoord));
                            }
                        }
                    }   
                }
            }
        }

        public List<Move> GetPossibleMoves()
        {
            //TO MODIFY: Take into account Split moves
            List<Move> list = new List<Move>();
            List<Pawn> ourPawns = this.OurPawns();
            foreach(Pawn pawn in ourPawns)
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
        /// Envoie les positions de nos pions sous forme de dictionnaire 
        /// key:Coord, value:(nombre de pions) 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Coord, int> OurPositions()
        {
            Dictionary<Coord, int> dict = new Dictionary<Coord,int>();

            foreach (Pawn pawn in Grid.Pawns) {
                if (pawn.Race.Equals(Race.US))
                {
                    dict.Add(pawn.Coordinates, pawn.Quantity);
                }
            }
            return dict;
        }

        /// <summary>
        /// Envoie nos pions sous forme de liste
        /// </summary>
        /// <returns></returns>
        public List<Pawn> OurPawns()
        {
            List<Pawn> list = new List<Pawn>();

            foreach (Pawn pawn in Grid.Pawns)
            {
                if (pawn.Race.Equals(Race.US))
                {
                    list.Add(pawn);
                }
            }
            return list;
        }

        /// <summary>
        /// Envoie les positions de les pions des ennemis sous forme de dictionnaire 
        /// key:Coord, value:(nombre de pions)
        /// </summary>
        /// <returns></returns>
        public Dictionary<Coord, int> EnnemyPositions()
        {
            Dictionary<Coord, int> dict = new Dictionary<Coord, int>();
            foreach (Pawn pawn in Grid.Pawns)
            {
                if (pawn.Race.Equals(Race.THEM))
                {
                    if(dict.ContainsKey(pawn.Coordinates)){
                        dict.Remove(pawn.Coordinates);
                    }
                    dict.Add(pawn.Coordinates, pawn.Quantity);
                }
            }
            return dict;
        }

        public Dictionary<Coord, int> HumanPositions()
        {
            Dictionary<Coord, int> dict = new Dictionary<Coord, int>();
            foreach (Pawn pawn in Grid.Pawns)
            {
                if (pawn.Race.Equals(Race.HUM))
                {
                    dict.Add(pawn.Coordinates, pawn.Quantity);
                }
            }
            return dict;
        }

        public int HumanNumber()
        {
            int number = 0;
            foreach (Pawn pawn in Grid.Pawns)
            {
                if (pawn.Race.Equals(Race.HUM))
                {
                    number += pawn.Quantity;
                }
            }
            return number;
        }

        public int OurNumber()
        {
            int number = 0;
            foreach (Pawn pawn in Grid.Pawns)
            {
                if (pawn.Race.Equals(Race.US))
                {
                    number += pawn.Quantity;
                }
            }
            return number;
        }

        public int EnnemyNumber()
        {
            int number = 0;
            foreach (Pawn pawn in Grid.Pawns)
            {
                if (pawn.Race.Equals(Race.THEM))
                {
                    number += pawn.Quantity;
                }
            }
            return number;
        }
    }
}