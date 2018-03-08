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
        private Dictionary<Tuple<int, int>, Tuple<char, int>> _grid;
        private int _x_max;
        private int _y_max;
        
        public Board(Dictionary<Tuple<int, int>, Tuple<char, int>> grid, int x_max, int y_max)
        {
            ///<summary>
            ///Constructeur pour définir une Board avec les dimensions et les positions des pions
            ///</summary>
            this._grid = grid;
            this._x_max = x_max;
            this._y_max = y_max;
        }

        public Dictionary<Tuple<int, int>, Tuple<char, int>> OurPositions()
        {
            ///<summary>
            ///Envoie les positions de nos pions sous forme de dictionnaire key:position, value:('n',nombre de pions)
            ///</summary>
            Dictionary<Tuple<int, int>, Tuple<char, int>> dict = new Dictionary<Tuple<int, int>, Tuple<char, int>>();
            // 'n' pour nous 

            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> k in _grid) {
                if (k.Value.Item1.Equals('n')) // mettre config.nous
                {
                    dict.Add(k.Key, k.Value);
                }
            }
            return dict;
        }

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

        public Dictionary<Tuple<int, int>, Tuple<char, int>> GetAdjacentPositions(Tuple<int, int> targetPosition) {
            //prend un tuple de position en entrée en renvoie les tuples adjacents sous forme d'un dico
            Dictionary<Tuple<int, int>, Tuple<char, int>> result = new Dictionary<Tuple<int, int>, Tuple<char, int>>();
            //on commence par mettre toutes les cases adjacentes, puis on vérifie si elle est bien dans la grid, on met ('h',0) dedans s'il n'y a rien ;)
            result.Add(new Tuple<int, int>(targetPosition.Item1+1, targetPosition.Item2-1), new Tuple<char, int>('h', 0));
            result.Add(new Tuple<int, int>(targetPosition.Item1+1, targetPosition.Item2), new Tuple<char, int>('h', 0));
            result.Add(new Tuple<int, int>(targetPosition.Item1+1, targetPosition.Item2-1), new Tuple<char, int>('h', 0));
            result.Add(new Tuple<int, int>(targetPosition.Item1, targetPosition.Item2+1), new Tuple<char, int>('h', 0));
            result.Add(new Tuple<int, int>(targetPosition.Item1, targetPosition.Item2-1), new Tuple<char, int>('h', 0));
            result.Add(new Tuple<int, int>(targetPosition.Item1-1, targetPosition.Item2+1), new Tuple<char, int>('h', 0));
            result.Add(new Tuple<int, int>(targetPosition.Item1-1, targetPosition.Item2), new Tuple<char, int>('h', 0));
            result.Add(new Tuple<int, int>(targetPosition.Item1-1, targetPosition.Item2-1), new Tuple<char, int>('h', 0));
            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> k in result)
            {
                if (k.Key.Item1<0 || k.Key.Item1 >= _x_max || k.Key.Item2<0 ||k.Key.Item2>=_y_max ) // mettre config.nous
                {
                    result.Remove(k.Key);
                }
                else
                {
                    if (_grid.ContainsKey(k.Key))
                    {
                        result[k.Key] = _grid[k.Key];
                    }
                }
            }
            return result;
        }

        public Dictionary<Tuple<int, int>, Tuple<char, int>> EnnemyPositions()
        {
            Dictionary<Tuple<int, int>, Tuple<char, int>> dict = new Dictionary<Tuple<int, int>, Tuple<char, int>>();
            // 'v' pour vous 

            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> k in _grid)
            {
                if (k.Value.Item1.Equals('v')) // mettre config.vous
                {
                    dict.Add(k.Key, k.Value);
                }
            }
            return dict;
        }

        public Dictionary<Tuple<int, int>, Tuple<char, int>> HumanPositions()
        {
            Dictionary<Tuple<int, int>, Tuple<char, int>> dict = new Dictionary<Tuple<int, int>, Tuple<char, int>>();
            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> k in _grid)
            {
                if (k.Value.Item1.Equals('h')) // mettre config.human
                {
                    dict.Add(k.Key, k.Value);
                }
            }
            return dict;
        }

        private int HumanNumber()
        {
            int number = 0;
            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> k in HumanPositions())
            {
                number += k.Value.Item2;
            }
            return number;
        }

        public int OurNumber()
        {
            int number = 0;
            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> k in OurPositions())
            {
                number += k.Value.Item2;
            }
            return number;
        }
        public int EnnemyNumber()
        {
            int number = 0;
            foreach (KeyValuePair<Tuple<int, int>, Tuple<char, int>> k in EnnemyPositions())
            {
                number += k.Value.Item2;
            }
            return number;
        }
    }
}

/* 




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