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
                
            }

            return dict;

        }

    }
}



/* 


    def our_positions(self):
        """
        formerly getour_positions(any_board)
        entree: board, nous
        retourne: liste de tuples qui nous donne nos positions et notre nombre sur ces positions
        NB: ne verifie pas la validite des coordonees
        """
        #print '\n'+50*'#'+"Board::our_positions()"
        our_positions =[]

        for k in self.grid.keys(): 
            if self.grid[k][0] == config.nous: 
                our_positions.append(VectorPosition(self.grid[k][0],k,self.grid[k][1]))
        return our_positions


    def ennemy_positions(self):
        """
        formerly getEnnemyPositions(any_board)
        entree: board, 
        retourne: liste de tuples qui nous donne la position des ennemis et leur nombre sur ces positions
        NB: ne verifie pas la validite des coordonees
        """
        #print '\n'+50*'#'+"Board::ennemy_positions()"
        ennemy_positions = []
        for k in self.grid.keys(): 
            if self.grid[k][0] == config.eux: 
                ennemy_positions.append(VectorPosition(self.grid[k][0],k,self.grid[k][1]))
        return ennemy_positions


    def human_positions(self):
        """
        formerly gethuman_positions(any_board)
        entree: board
        retourne: liste de tuples qui nous donne la position des humains et leur nombre sur ces positions
        NB: ne verifie pas la validite des coordonees
        """
        #print '\n'+50*'#'+"Board::human_positions()"
        human_positions = []
        for k in self.grid.keys(): 
            if self.grid[k][0] == 'h': 
                human_positions.append(VectorPosition(self.grid[k][0],k,self.grid[k][1]))
        return human_positions

    def human_number(self): #to be checked
        """
        formerly getHumanNumber(any_board)
        entree: board
        retourne le nombre d'humains restants sur le plateau
        """
        #print '\n'+50*'#'+"Board::human_number()"
        #rappel: board[(x,y)]=(type, nombre)
        return sum([v[1] for k,v in self.grid.items() if v[0]=='h'])



    def our_number(self): #to be checked
        """
        formerly getOurNumber(any_board)
        """
        #print '\n'+50*'#'+"Board::our_number()"
        return sum([v[1] for k,v in self.grid.items() if v[0]==config.nous])

    def ennemy_number(self): #to be checked
        """
        formerly getEnnemyNumber(any_board) 
        retourne le nombre d'ennemis restants sur le plateau
        """
        #print '\n'+50*'#'+"Board::ennemy_number()"
        return sum([v[1] for k,v in self.grid.items() if v[0]==config.eux])



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