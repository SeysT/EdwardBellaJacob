using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    class Board
    // Class to handle boards
    {
        public Grid Grid { get; private set; }
        private int _x_max;
        private int _y_max;
        
        public Board(Board b)
        {
            this.Grid = new Grid(b.Grid);
            this._x_max = b._x_max;
            this._y_max = b._y_max;
        }

        public Board(Grid grid, int x_max, int y_max)
        {
            this.Grid = new Grid(grid);
            this._x_max = x_max;
            this._y_max = y_max;
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
                        else if(inNewCoord.Quantity <= 1.5 * move.Quantity)
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
                Dictionary<Coord, Direction> adjCoords = this.GetAdjacentCoordAndDir(pawn.Coordinates);
                foreach (Direction direction in adjCoords.Values)
                {
                    list.Add(new Move(pawn.Coordinates, direction, pawn.Quantity));
                }
            }
            return list;
        }

        public float GetHeuristicScore()
        {
            return new Heuristic(this).GetScore(0.3f, 0.1f, 0.2f, 0.2f, 0.2f);
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

        public List<Pawn> GetAdjacentPositions(Coord targetPosition) {
            // prend un tuple de position en entrée en renvoie les tuples adjacents sous forme d'un dico
            List<Pawn> result = new List<Pawn>
            {
                // on commence par mettre toutes les cases adjacentes, 
                // puis on vérifie si elle est bien dans la grid, on met ('h', 0) dedans s'il n'y a rien ;)
                new Pawn(Race.HUM, 0, targetPosition.X + 1, targetPosition.Y - 1),
                new Pawn(Race.HUM, 0, targetPosition.X + 1, targetPosition.Y),
                new Pawn(Race.HUM, 0, targetPosition.X + 1, targetPosition.Y + 1),
                new Pawn(Race.HUM, 0, targetPosition.X, targetPosition.Y - 1),
                new Pawn(Race.HUM, 0, targetPosition.X, targetPosition.Y + 1),
                new Pawn(Race.HUM, 0, targetPosition.X - 1, targetPosition.Y + 1),
                new Pawn(Race.HUM, 0, targetPosition.X - 1, targetPosition.Y - 1),
                new Pawn(Race.HUM, 0, targetPosition.X - 1, targetPosition.Y)
            };

            for (int i = result.Count - 1; i <= 0; i--)
            {
                Coord coord = result[i].Coordinates;
                if (coord.X < 0
                    || coord.X >= _x_max
                    || coord.Y < 0
                    || coord.Y >= _y_max)
                {
                    result.Remove(result[i]);
                }
                else
                {
                    result[i] = Grid.GetInCoord(result[i].Coordinates);
                }
            }
            return result;
        }

        public Dictionary<Coord, Direction> GetAdjacentCoordAndDir(Coord targetPosition)
        {
            // prend un tuple de position en entrée en renvoie les tuples adjacents sous forme d'un dico
            Dictionary<Coord, Direction> result = new Dictionary<Coord, Direction>
            {
                // on commence par mettre toutes les cases adjacentes, 
                // puis on vérifie si elle est bien dans la grid, on met ('h', 0) dedans s'il n'y a rien ;)
                { new Coord(targetPosition.X + 1, targetPosition.Y - 1), Direction.UR },
                { new Coord(targetPosition.X + 1, targetPosition.Y), Direction.R },
                { new Coord(targetPosition.X + 1, targetPosition.Y + 1), Direction.DR },
                { new Coord(targetPosition.X, targetPosition.Y - 1), Direction.U },
                { new Coord(targetPosition.X, targetPosition.Y + 1), Direction.D },
                { new Coord(targetPosition.X - 1, targetPosition.Y + 1), Direction.DL },
                { new Coord(targetPosition.X - 1, targetPosition.Y - 1), Direction.UL },
                { new Coord(targetPosition.X - 1, targetPosition.Y), Direction.L }
            };


            for (int i = result.Keys.Count - 1; i <= 0; i--)
            {
                Coord coord = result.Keys.ToList()[i];
                if (coord.X < 0
                    || coord.X >= _x_max
                    || coord.Y < 0
                    || coord.Y >= _y_max)
                {
                    result.Remove(coord);
                }
            }
            return result;
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