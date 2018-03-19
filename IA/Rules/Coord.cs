using System.Collections.Generic;

namespace IA.Rules
{
    class Coord
    {
        public Coord(Coord c)
        {
            this.X = c.X;
            this.Y = c.Y;
        }

        public Coord(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public Coord(int[,] coords)
        {
            X = coords[0, 0];
            Y = coords[1, 0];
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var coord = obj as Coord;
            return coord != null &&
                   X == coord.X &&
                   Y == coord.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        static public Dictionary<Coord, Direction> PossibleCoordsFromDirectionMoves(Coord coord)
        {
            return new Dictionary<Coord, Direction>
            {
                { new Coord(coord.X + 1, coord.Y - 1), Direction.UR },
                { new Coord(coord.X + 1, coord.Y), Direction.R },
                { new Coord(coord.X + 1, coord.Y + 1), Direction.DR },
                { new Coord(coord.X, coord.Y - 1), Direction.U },
                { new Coord(coord.X, coord.Y + 1), Direction.D },
                { new Coord(coord.X - 1, coord.Y + 1), Direction.DL },
                { new Coord(coord.X - 1, coord.Y - 1), Direction.UL },
                { new Coord(coord.X - 1, coord.Y), Direction.L }
            };
        }

        static public Coord DirectionMove(Coord coord, Direction direction)
        {
            Coord newCoord = new Coord(coord);
            switch (direction)
            {
                case Direction.D:
                    newCoord.Y += 1;
                    break;
                case Direction.U:
                    newCoord.Y -= 1;
                    break;
                case Direction.R:
                    newCoord.X += 1;
                    break;
                case Direction.L:
                    newCoord.X -= 1;
                    break;
                case Direction.UR:
                    newCoord.X += 1;
                    newCoord.Y -= 1;
                    break;
                case Direction.UL:
                    newCoord.X -= 1;
                    newCoord.Y -= 1;
                    break;
                case Direction.DL:
                    newCoord.Y += 1;
                    newCoord.X -= 1;
                    break;
                case Direction.DR:
                    newCoord.Y += 1;
                    newCoord.X += 1;
                    break;
                default:
                    break;
            }

            return newCoord;
        }
    }
}
