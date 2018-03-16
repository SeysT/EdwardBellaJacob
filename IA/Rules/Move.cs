using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    enum Direction { UL, U, UR, L, R, DL, D, DR }

    class Move
    {
        public Coord Coordinates { get; set; }
        public Direction Direction { get; set; }
        public int Quantity { get; set; }

        public Move(Move m)
        {
            this.Coordinates = new Coord(m.Coordinates);
            this.Direction = m.Direction;
            this.Quantity = m.Quantity;
        }
        /// <summary>
        /// Constructeur de Move: qui prend en entrée les coordonnées, la direction 
        /// et le nombre de pions à bouger
        /// </summary>
        public Move(Coord coord, Direction dir, int q)
        {
            this.Coordinates = new Coord(coord);
            this.Direction = dir;
            this.Quantity = q;
        }

        public Move(int x, int y, Direction dir, int q)
        {
            this.Coordinates = new Coord(x, y);
            this.Direction = dir;
            this.Quantity = q;
        }
    }
}