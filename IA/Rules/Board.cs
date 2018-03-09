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
        private Grid _grid;
        private int _x_max;
        private int _y_max;
        
        public Board(Grid grid, int x_max, int y_max)
        {
            this._grid = grid;
            this._x_max = x_max;
            this._y_max = y_max;
        }

        public Board MakeMove(List<Move> moves)
        {
            Board newBoard = this;
            newBoard.UpdateTable(moves);
            return newBoard;
        }

        public void UpdateTable(List<Move> moves)
        {
            foreach (Move move in moves)
            {
                Pawn p = _grid._getInCoord(move._c);
                if (p._quantity.Equals(move._nbre) && !p._quantity.Equals(0))
                {
                    _grid.Pawns.Remove(p);
                }
                else
                {
                    _grid._setQuantityInCoord(p._coordinates,p._quantity - move._nbre);
                    Coord new_coord = p._coordinates;
                    switch (move._dir)
                    {
                        case Direction.D:
                            new_coord.Y += 1;
                            break;
                        case Direction.U:
                            new_coord.Y -= 1;
                            break;
                        case Direction.R:
                            new_coord.X += 1;
                            break;
                        case Direction.L:
                            new_coord.X -= 1;
                            break;
                        case Direction.UR:
                            new_coord.X += 1;
                            new_coord.Y -= 1;
                            break;
                        case Direction.UL:
                            new_coord.X -= 1;
                            new_coord.Y -= 1;
                            break;
                        case Direction.DL:
                            new_coord.Y += 1;
                            new_coord.X -= 1;
                            break;
                        case Direction.DR:
                            new_coord.Y += 1;
                            new_coord.X += 1;
                            break;
                    }

                    _grid.Pawns.Add(new Pawn(p._type,move._nbre, new_coord));
                }
            }
        }

        public List<Move> GetPossibleMoves()
        {
            //TO MODIFY: Take into account Split moves
            List<Move> list = new List<Move>();
            List<Pawn> ourPawns = this.OurPawns();
            foreach(Pawn p in ourPawns)
            {
                Dictionary<Coord,Direction> AdjCoord=this.GetAdjacentCoordAndDir(p._coordinates);
                foreach (Coord coord in AdjCoord.Keys)
                {
                    Move move = new Move(coord, AdjCoord[coord], p._quantity);
                    //nbre = p._quantity Moving all the pawns
                }
            }
            return list;
        }

        public float GetHeuristiqueScore()
        {
            //TODO
            return 0;
        }

        public Dictionary<Coord, int> OurPositions()
        {
            ///<summary>
            ///Envoie les positions de nos pions sous forme de dictionnaire 
            ///key:Coord, value:(nombre de pions)
            ///</summary>
            Dictionary<Coord, int> dict = new Dictionary<Coord,int>();
            // 'n' pour nous 

            foreach (Pawn pawn in _grid.Pawns) {
                if (pawn._type.Equals('n')) // mettre config.nous
                {
                    dict.Add(pawn._coordinates, pawn._quantity);
                }
            }
            return dict;
        }
        public List<Pawn> OurPawns()
        {
            ///<summary>
            ///Envoie nos pions sous forme de liste
            ///</summary>
            List<Pawn> list = new List<Pawn>();
            // 'n' pour nous 

            foreach (Pawn pawn in _grid.Pawns)
            {
                if (pawn._type.Equals('n')) // mettre config.nous
                {
                    list.Add(pawn);
                }
            }
            return list;
        }


        public List<Pawn> GetAdjacentPositions(Coord targetPosition) {
            //prend un tuple de position en entrée en renvoie les tuples adjacents sous forme d'un dico
            List<Pawn> result = new List<Pawn>();
            ///on commence par mettre toutes les cases adjacentes, 
            ///puis on vérifie si elle est bien dans la grid, on met ('h',0) dedans s'il n'y a rien ;)
            result.Add(new Pawn('h', 0, targetPosition.X + 1, targetPosition.Y - 1));
            result.Add(new Pawn('h', 0, targetPosition.X + 1, targetPosition.Y ));
            result.Add(new Pawn('h', 0, targetPosition.X + 1, targetPosition.Y + 1));
            result.Add(new Pawn('h', 0, targetPosition.X , targetPosition.Y - 1));
            result.Add(new Pawn('h', 0, targetPosition.X , targetPosition.Y + 1));
            result.Add(new Pawn('h', 0, targetPosition.X - 1, targetPosition.Y + 1));
            result.Add(new Pawn('h', 0, targetPosition.X - 1, targetPosition.Y - 1));
            result.Add(new Pawn('h', 0, targetPosition.X - 1, targetPosition.Y));

            foreach (Pawn pawn in result)
            {
                if (pawn._coordinates.X<0 
                    || pawn._coordinates.X >= _x_max 
                    || pawn._coordinates.Y < 0 
                    || pawn._coordinates.Y >= _y_max ) // mettre config.nous
                {
                    result.Remove(pawn);
                }
                else
                {
                    Pawn pawnVol = _grid._getInCoord(pawn._coordinates);
                    if (pawnVol._quantity.Equals(0))
                    {
                        result.Remove(pawn);
                        result.Add(new Pawn(pawnVol));
                    }
                }
            }
            return result;
        }


        public Dictionary<Coord,Direction> GetAdjacentCoordAndDir(Coord targetPosition)
        {
            //prend un tuple de position en entrée en renvoie les tuples adjacents sous forme d'un dico
            Dictionary<Coord, Direction> result = new Dictionary<Coord, Direction>();
            ///on commence par mettre toutes les cases adjacentes, 
            ///puis on vérifie si elle est bien dans la grid, on met ('h',0) dedans s'il n'y a rien ;)
            result.Add(new Coord(targetPosition.X + 1, targetPosition.Y - 1), Direction.UR);
            result.Add(new Coord( targetPosition.X + 1, targetPosition.Y),Direction.R);
            result.Add(new Coord( targetPosition.X + 1, targetPosition.Y + 1),Direction.DR);
            result.Add(new Coord( targetPosition.X, targetPosition.Y - 1),Direction.U);
            result.Add(new Coord( targetPosition.X, targetPosition.Y + 1),Direction.D);
            result.Add(new Coord( targetPosition.X - 1, targetPosition.Y + 1),Direction.DL);
            result.Add(new Coord( targetPosition.X - 1, targetPosition.Y - 1),Direction.UL);
            result.Add(new Coord( targetPosition.X - 1, targetPosition.Y),Direction.L);

            foreach (Coord coord in result.Keys)
            {
                if (coord.X < 0
                    || coord.X >= _x_max
                    || coord.Y < 0
                    || coord.Y >= _y_max) // mettre config.nous
                {
                    result.Remove(coord);
                }
            }
            return result;
        }

        public Dictionary<Coord, int> EnnemyPositions()
        {
            ///<summary>
            ///Envoie les positions de les pions des ennemis sous forme de dictionnaire 
            ///key:Coord, value:(nombre de pions)
            ///</summary>
            Dictionary<Coord, int> dict = new Dictionary<Coord, int>();
            foreach (Pawn pawn in _grid.Pawns)
            {
                if (pawn._type.Equals('v')) // mettre config.nous
                {
                    dict.Add(pawn._coordinates, pawn._quantity);
                }
            }
            return dict;
        }

        public Dictionary<Coord, int> HumanPositions()
        {
            Dictionary<Coord, int> dict = new Dictionary<Coord, int>();
            foreach (Pawn pawn in _grid.Pawns)
            {
                if (pawn._type.Equals('h')) // mettre config.
                {
                    dict.Add(pawn._coordinates, pawn._quantity);
                }
            }
            return dict;
        }

        private int HumanNumber()
        {
            int number = 0;
            foreach (Pawn pawn in _grid.Pawns)
            {
                if (pawn._type.Equals('h')) // mettre config.human
                {
                    number += pawn._quantity;
                }
            }
            return number;
        }

        public int OurNumber()
        {
            int number = 0;
            foreach (Pawn pawn in _grid.Pawns)
            {
                if (pawn._type.Equals('n')) // mettre config.human
                {
                    number += pawn._quantity;
                }
            }
            return number;
        }
        public int EnnemyNumber()
        {
            int number = 0;
            foreach (Pawn pawn in _grid.Pawns)
            {
                if (pawn._type.Equals('v')) // mettre config.human
                {
                    number += pawn._quantity;
                }
            }
            return number;
        }
    }
}

/* 

            public Boolean CheckPresence(Dictionary<Tuple<int, int>, Tuple<char, int>> adjacentsDict)
        {
            ///<summary>
            ///envoie true s'il y a des pions 'v' ou 'h' dans des cases adjacentes aux pions 'n'
            ///</summary>
            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> tuplePosition in adjacentsDict)
            {
                if (_grid.ContainsKey(tuplePosition.Key)) {
                    if (_grid[tuplePosition.Key].Item1.Equals('n')) // mettre config.nous
                    {
                        return true;
                    }
                }
            }
            return false;
        }



    def ennemy_close(self):
        """
        formerly anyEnnemyClose(any_board)
        retourne: un dico avec en clé: tuple de positions des ennemis qui snt adjacents à une de nos positions, et en valeur un tuple (tuples de nos positions mmenacées, nombre de nos creatures présentes sur cette case)
        """
        #print '\n'+50*'#'+"Board::ennemy_close()"
        our_positions = self.our_positions()
        ennemy_positions = self.ennemy_positions()
        allDistances = []
        for our_position in our_positions:
            for ennemy_position in ennemy_positions:
                distance = computeMinDistance(our_position.coord, ennemy_position.coord)
                allDistances.append((our_position, ennemy_position, distance))
        return sorted(allDistances, key=lambda distance: distance[2])



    def human_close(self):
        """
        formerly anyHumanClose(any_board)
        retourne: un dico avec en clé: tuple de positions des humains qui snt adjacents à une de nos positions, et en valeur un tuple (tuples de nos positions mmenacées, nombre de nos creatures présentes sur cette case)
        """
        #print '\n'+50*'#'+"Board::human_close()"
        our_positions = self.our_positions()
        human_positions = self.human_positions()
        allDistances = []
        for our_position in our_positions:
            for human_position in human_positions:
                distance = computeMinDistance(our_position.coord, human_position.coord)
                allDistances.append((our_position, human_position, distance))
        return sorted(allDistances, key=lambda distance: distance[2])



    def sum_min_distance_us_human_delta(self):
        """
        formerly sum_min_distance_us_human_delta(any_board)
        """
        #print '\n'+50*'#'+"Board::sum_min_distance_us_human_delta()"
        dist = 0
        our_positions = self.our_positions() #[((x,y),number)]
        human_positions = self.human_positions() #[((x,y),number)]
        if human_positions:
            for our_position in our_positions:
                local_dist= float("inf")
                local_coef=0 #will be set to +1 if humans outnumber us
                for human_position in human_positions:
                    if computeMinDistance(our_position.coord,human_position.coord) < local_dist:
                        local_dist = computeMinDistance(our_position.coord,human_position.coord)
                        if our_position.number<=human_position.number:
                            local_coef = 1
                        else:
                            local_coef = -1
                dist+=local_dist*local_coef
        return dist

    */
