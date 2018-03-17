using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using IA.Rules;
using System.Collections.Generic;
using System;

namespace IA
{
    class Client
    {
        private string _host;
        private int _port;

        private Socket _socket;
        private Stopwatch _sw;

        private string _playerName = "EdwardBellaJacob";
        private ServerPlayerTrame _trame;

        private Board _board;
        private Dictionary<Race, int> _indexes;

        public Client(string host, int port)
        {
            this._host = host;
            this._port = port;

            this._socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this._trame = new ServerPlayerTrame(this._socket);

            this._indexes = new Dictionary<Rules.Race, int>() { {Race.HUM, 2}, {Race.THEM, 0}, {Race.US, 0} };

            this._sw = new Stopwatch();
        }

        private void _initGame() {
            // First trame has already been received outside _initGame()
            int[,] boardSize = this._trame.TramePayload;
            ServerPlayerTrame.CheckTrameType(this._trame, "SET");

            // We don't need human coordinates to init game
            this._trame.Receive();
            ServerPlayerTrame.CheckTrameType(this._trame, "HUM");

            // We get our starting coordinates
            Coord hmeCoords = new Coord(this._trame.Receive());
            ServerPlayerTrame.CheckTrameType(this._trame, "HME");

            // We get all information from map
            int[,] mapInfos = this._trame.Receive();
            ServerPlayerTrame.CheckTrameType(this._trame, "MAP");

            // We init the game
            Grid grid = new Grid();
            for (int i = 0; i < mapInfos.GetLength(0); i++)
            {
                Coord coords = new Coord(mapInfos[i, 0], mapInfos[i, 1]);
                if (mapInfos[i, this._indexes[Race.HUM]] != 0)
                {
                    grid.Add(new Pawn(Race.HUM, mapInfos[i, this._indexes[Race.HUM]], coords));
                }
                else
                {
                    if (hmeCoords.Equals(coords))
                    {
                        this._indexes[Race.US] = mapInfos[i, 3] != 0 ? 3 : 4;
                        grid.Add(new Pawn(Race.US, mapInfos[i, this._indexes[Race.US]], coords));
                    }
                    else
                    {
                        this._indexes[Race.THEM] = mapInfos[i, 3] != 0 ? 3 : 4;
                        grid.Add(new Pawn(Race.THEM, mapInfos[i, this._indexes[Race.THEM]], coords));
                    }
                }
            }
            this._board = new Board(grid, boardSize[1, 0], boardSize[0, 0]);
        }

        private int[, ] _chooseMove()
        { 
            Node root = new Node(new NodeData());
            List<Move> moveCandidates = this._board.GetPossibleMoves();
            float value = new MinMax().AlphaBeta(root, 357, float.MinValue, float.MaxValue, true, moveCandidates, this._board);
            List<Move> moves = MinMax.GetNextMove(root, value);

            return MOVTrame.GetPayloadFromMoves(moves);
        }

        private void _updateGame()
        {
            int[,] updates = this._trame.TramePayload;
            for (int i = 0; i < updates.GetLength(0); i++)
            {
                Coord coord = new Coord(updates[i, 0], updates[i, 1]);

                // Get race from mapInfos trame :
                // if we find a non null pawn we get its race else we get the non null quantity in updates
                Pawn pawn = this._board.Grid.GetInCoord(coord);
                Race race = pawn.Race;
                if (pawn.Race == Race.HUM && pawn.Quantity == 0)
                {
                    race = updates[i, this._indexes[Race.HUM]] != 0 ? Race.HUM : updates[i, this._indexes[Race.US]] != 0 ? Race.US : Race.THEM;
                }

                // Update Board
                this._board.Grid.SetQuantityInCoord(coord, updates[i, this._indexes[race]], race);
            }
        }

        public void Start()
        {
            this._socket.Connect(this._host, this._port);

            new NMETrame(this._playerName).Send(this._socket);

            this._trame.Receive();
            this._initGame();

            bool finish = false;
            while (!finish)
            {
                this._trame.Receive();

                switch (this._trame.TrameHeader)
                {
                    case "UPD":
                        this._sw.Start();

                        this._updateGame();
                        int[,] next = this._chooseMove();

                        this._sw.Stop();
                        Console.WriteLine($"Elapsed Time={this._sw.Elapsed}");

                        new MOVTrame(next).Send(this._socket);
                        break;
                    case "END":
                        this._trame.Receive();
                        if (this._trame.TrameHeader == "BYE")
                        {
                            finish = true;
                        } else
                        {
                            this._initGame();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}