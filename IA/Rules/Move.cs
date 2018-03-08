using System.Text;
using System.Threading.Tasks;

namespace IA.Rules
{
    enum Direction { UL, U, UR, L, R, DL, D, DR }

    class Move
    {
        private Coord _c;
        private Direction _dir;
        private int _nbre;

        /// <summary>
        /// Constructeur de Move: qui prend en entrée les coordonnées, la direction 
        /// et le nombre de pions à bouger
        /// </summary>
        public Move(Coord initialCoord, Direction dir, int q)
        {
            this._c = initialCoord;
            this._dir = dir;
            this._nbre = q;
        }

        public Move(int x, int y, Direction dir, int q)
        {
            this._c = new Coord(x, y);
            this._dir = dir;
            this._nbre = q;
        }
    }
}