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

        private  Dictionary<Tuple<int, int>, Tuple<string, int>> _grid;
        private int _x_max;
        private int _y_max;
         
        public Board(Dictionary<Tuple<int,int>, Tuple<string,int>> grid, int x_max , int y_max)
        {
            this._grid = grid;
            this._x_max = x_max;
            this._y_max = y_max;
        }
        private Dictionary<Tuple<int, int>, Tuple<string, int>> Our_Positions()
        {
            Dictionary<Tuple<int, int>, Tuple<string, int>> dict = new Dictionary<Tuple<int, int>, Tuple<string, int>>();
            // 'n' pour nous 
            
            foreach(KeyValuePair<Tuple<int, int>, Tuple<string, int>> k in _grid){
                if (k.Value.Item1.Equals('n')) // mettre config.nous
                {
                    dict.Add(k.Key,k.Value);
                }
            }
            return dict;
        }
        private Dictionary<Tuple<int, int>, Tuple<string, int>> Ennemy_positions()
        {
            Dictionary<Tuple<int, int>, Tuple<string, int>> dict = new Dictionary<Tuple<int, int>, Tuple<string, int>>();
            // 'v' pour vous 

            foreach (KeyValuePair<Tuple<int, int>, Tuple<string, int>> k in _grid)
            {
                if (k.Value.Item1.Equals('v')) // mettre config.vous
                {
                    dict.Add(k.Key, k.Value);
                }
            }
            return dict;
        }
        private Dictionary<Tuple<int, int>, Tuple<string, int>> Human_positions()
        {
            Dictionary<Tuple<int, int>, Tuple<string, int>> dict = new Dictionary<Tuple<int, int>, Tuple<string, int>>();
            foreach (KeyValuePair<Tuple<int, int>, Tuple<string, int>> k in _grid)
            {
                if (k.Value.Item1.Equals('h')) // mettre config.human
                {
                    dict.Add(k.Key, k.Value);
                }
            }
            return dict;
        }
        private int Human_number()
        {
            Dictionary<Tuple<int, int>, Tuple<string, int>> dict = new Dictionary<Tuple<int, int>, Tuple<string, int>>();
            int number=0;
            foreach (KeyValuePair<Tuple<int, int>, Tuple<string, int>> k in Human_positions())
            {
                number+= k.Value.Item2;
            }
            return number;
        }
        private int Our_number()
        {
            Dictionary<Tuple<int, int>, Tuple<string, int>> dict = new Dictionary<Tuple<int, int>, Tuple<string, int>>();
            int number = 0;
            foreach (KeyValuePair<Tuple<int, int>, Tuple<string, int>> k in Our_Positions())
            {
                number += k.Value.Item2;
            }
            return number;
        }
        private int Ennemy_number()
        {
            Dictionary<Tuple<int, int>, Tuple<string, int>> dict = new Dictionary<Tuple<int, int>, Tuple<string, int>>();
            int number = 0;
            foreach (KeyValuePair<Tuple<int, int>, Tuple<string, int>> k in Ennemy_positions())
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