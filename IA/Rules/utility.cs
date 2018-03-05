using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    public class Utility
    {
        public float ComputeMinDistance(Tuple<int, int> coord_1, Tuple<int, int> coord_2)
        {

            return Math.Max(Math.Abs(coord_2.Item1 - coord_1.Item1), Math.Abs(coord_2.Item2 - coord_1.Item2));
        }
    }

    public Tuple<int, int> FindNextMove(Tuple<int, int> coord_start, Tuple<int, int> coord_goal) {
        //retourne la prochaine case sur laquelle aller à partir d'une coordonee de depart et une coordonee cible
        if ((coord_start.Item1 == coord_goal.Item1) && (coord_start.Item2 == coord_goal.Item2))
        {
            return  coord_start;
        }
        else if (coord_start.Item1 == coord_goal.Item1) {
            if (coord_start.Item2 < coord_goal.Item2)
            {
                return new Tuple<int, int>(coord_start.Item1, coord_start.Item2 + 1);
            }
            else
            {
                return new Tuple<int, int>(coord_start.Item1, coord_start.Item2 - 1);
            }
        }
        else if (coord_start.Item2 == coord_goal.Item2) {
            if (coord_start.Item1 < coord_goal.Item1)
            {
                return new Tuple<int, int>(coord_start.Item1 + 1, coord_start.Item2);
            }
            else
            {
                return new Tuple<int, int>(coord_start.Item1 - 1, coord_start.Item2);
            }
        }
        else if (coord_start.Item1 < coord_goal.Item1) {
            if (coord_start.Item2 < coord_goal.Item2)
            {
                return new Tuple<int, int>(coord_start.Item1 + 1, coord_start.Item2 + 1);
            }
            else { return new Tuple<int, int>(coord_start.Item1 + 1, coord_start.Item2 - 1); }
            
        }
        else if (coord_start.Item1 > coord_goal.Item1) {
            if (coord_start.Item2 < coord_goal.Item2) {
                return new Tuple<int, int>(coord_start.Item1 - 1, coord_start.Item2 + 1)}
            else { return new Tuple<int, int>(coord_start.Item1 - 1, coord_start.Item2 - 1); }
            }
        else { throw new Exception("[Rules.Utility]FindNextMove: Le code n'est pas supposé arriver là"); }
    } 
}

/*



def move(coord_start,number,coord_end):
    """
    entree: une liste de quintuplets
    NB: checker ue le nombre de quintuplets est < à 3
    retourne: le send correctement formaté
    """
    return send(sock, "MOV", coord_start[0], coord_start[1], number,coord_end[0],coord_end[1])

def next_coord(coord_start, direction):
    """
    coord_start est le tuple de coordonnes
    "direction" est une direction de deplacement. 8 possibilités: u,ur,r,dr,d,dl,l,ul
    si le mouvement dans cette diection est possible, retourne ls cordonnes suivantes apres le mouvement
    sinon retourne False
    """

    Xsize = config.Xsize
    Ysize = config.Ysize

    if direction == 'u':
        print( "direction u")
        if coord_start[1]+1<=Ysize-1:
            return (coord_start[0], coord_start[1]+1)
        else:
            return coord_start

    elif direction == 'ur':
        if coord_start[0]+1<=Xsize-1 and coord_start[1]+1<=Ysize-1 :
            return (coord_start[0]+1, coord_start[1]+1)
        else:
            return coord_start

    elif direction == 'r':
        if coord_start[0]+1<=Xsize-1:
            return (coord_start[0]+1, coord_start[1])
        else:
            return coord_start
   

    elif direction == 'dr':
        if coord_start[0]+1<=Xsize-1 and coord_start[1]-1>=0:
            return (coord_start[0]+1, coord_start[1]-1)
        else:
            return coord_start
   

    elif direction == 'd':
        if coord_start[1]-1>=0:
            return (coord_start[0], coord_start[1]-1)
        else:
            return coord_start
   

    elif direction == 'dl':
        if coord_start[0]-1>=0 and coord_start[1]-1>=0:
            return (coord_start[0], coord_start[1]-1)
        else:
            return coord_start
   

    elif direction == 'l':
        if coord_start[0]-1>=0:
            return (coord_start[0], coord_start[1])
        else:
            return coord_start
   

    elif direction == 'ul':  
        if coord_start[0]-1>=0 and coord_start[1]+1<=Ysize-1:
            return (coord_start[0]-1, coord_start[1]+1)
        else:
            return coord_start




def randomPossibleNextCoord(coord_start):
    """
    Renvoie les coordonnées possibles (= qui ne sort pas de la carte) pour un next move aléatoire. 
    """
    print( "\nProcedure randomPossibleNextCoord")
    coord=coord_start
    print (coord)
    while (coord==coord_start):
            print( "debut d'une boucle de while")
            direction= choice(['u','ur','r','dr','d','dl','l','ul'])
            print (direction)
            coord = next_coord(coord_start, direction)
            print (coord)
    return coord



    

class VectorPosition():
    def __init__(self, kind, coord, number):
        self.kind = kind   # 'v', 'w' or 'h'
        self.coord = coord
        self.number = number
        self.x=coord[0]
        self.y=coord[1]


def main():
    """
    for testing purposes only
    this part is executed only when the file is executed in command line 
    (ie not executed when imported in another file)
    """
    grid = {(0,0):('h',5),(2,5):('v',4),(1,4):('w',3),(4,3):('h',2),(5,0):('h',3),(2,2):('v',4),(4,8):('w',5),(8,8):('w',2),(5,9):('v',4)}

    config.nous = 'v'
    config.eux = 'w'

    v = VectorPosition('v', (5,0),4)
    print (v.coord)


    #instanciate a board:
    board = Board(grid,10,10)

    #test methods:
    print ('-'*50)
    print ("Nombre de nos creatures: "+str(board.our_number()))
    print ("Nombre d'humains: "+str(board.human_number()))
    print ("Nombre d'ennemis: "+str(board.ennemy_number()))
    print ('-'*50)
    print ("Nos positions: " + str(board.our_positions()))
    print ("Notre premiere position: "+str(board.our_positions()[0].coord))
    print ("Les coordonnees de notre premiere position: "+str((board.our_positions()[0]).coord))
    print ('-'*50)
    print ("Nos ennemis proches sont: " + str(board.ennemy_close()))
    print ("Les humains proches sont: " + str(board.human_close()))
    print ('-'*50)
    print ("le score heuristique du board est: "+str(board.score()))


#for testing purposes only
if __name__=="__main__":
    main()
*/