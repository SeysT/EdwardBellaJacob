using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using IA.Rules;
using System.Collections.Generic;
using System;
using IA.IA;
using System.Configuration;
using System.Globalization;

namespace IA
{
    class Client
    {
        private string _host;
        private int _port;

        private Socket _socket;

        private BaseIA _iaNoSplit;
        private BaseIA _iaSplit;

        private string _playerName = "EdwardBellaJacob";
        private ServerPlayerTrame _trame;

        private Board _board;
        private Dictionary<Race, int> _indexes;

        private int[,] _next;
        private int[,] _nextSplit;
        private int[,] _nextNoSplit;

        public Client(string host, int port)
        {
            this._host = host;
            this._port = port;

            this._socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this._trame = new ServerPlayerTrame(this._socket);

            int.TryParse(ConfigurationManager.AppSettings["MinMaxDepthSplit"], out int depthSplit);
            int.TryParse(ConfigurationManager.AppSettings["MinMaxDepthNoSplit"], out int depthNoSplit);
            this._iaNoSplit = new MinMax(depthNoSplit, false);
            this._iaSplit = new MinMax(depthSplit, true);

            this._indexes = new Dictionary<Race, int>() { {Race.HUM, 2}, {Race.THEM, 0}, {Race.US, 0} };
            
            Trace.Listeners.Add(new TextWriterTraceListener($"Trace_{System.DateTime.Now.ToString("D")}.txt"));
            Trace.TraceInformation("Initialisation du client");
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

        private void _updateGame()
        {
            int[,] updates = this._trame.TramePayload;
            for (int i = 0; i < updates.GetLength(0); i++)
            {
                Coord coord = new Coord(updates[i, 0], updates[i, 1]);
                // Get race from mapInfos trame :
                Race race = updates[i, this._indexes[Race.HUM]] != 0 ? Race.HUM : updates[i, this._indexes[Race.US]] != 0 ? Race.US : Race.THEM;

                // Update Board
                // if quantity == 0, we will only remove the pawn that moved
                this._board.Grid.SetQuantityInCoord(coord, updates[i, this._indexes[race]], race);
            }
        }

        private int[,] _chooseMoveSplit()
        {
            return this._iaSplit.ChooseNextMove();
        }

        private int[,] _chooseMoveNoSplit()
        {
            return this._iaNoSplit.ChooseNextMove();
        }

        private void _setNextMoveSplit()
        {
            this._nextSplit = _chooseMoveSplit();
        }

        private void _setNextMoveNoSplit()
        {
            this._nextNoSplit = _chooseMoveNoSplit();
        }

        private void _computeMoveSplit()
        {
            this._iaSplit.ComputeNextMove(this._board);
        }

        private void _computeMoveNoSplit()
        {
            this._iaNoSplit.ComputeNextMove(this._board);
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
                        this._updateGame();
                        Thread threadSplit = new Thread(_computeMoveSplit);
                        Thread threadNoSplit = new Thread(_computeMoveNoSplit);
                        // previously the line below was here
                        // int[,] next = this._chooseMove();
                        // now it's split into two methods :
                        // _computeMove()
                        // _setNextMove()
                        
                        //threadSplit.Start();
                        threadNoSplit.Start();
                        //threadSplit.Join(700);
                        //threadNoSplit.Join();

                        Thread.Sleep(500);

                        if (_iaSplit.AlphaBetaFinished && _iaNoSplit.AlphaBetaFinished)
                        {
                            _setNextMoveSplit();
                            _setNextMoveNoSplit();
                            Console.WriteLine("Split&NoSplit finished");
                            _next = _iaSplit.score > _iaNoSplit.score ? _nextSplit : _nextNoSplit;
                        }
                        else if (_iaSplit.AlphaBetaFinished)
                        {
                            _setNextMoveSplit();
                            
                            Console.WriteLine("Split finished");
                            _next = _nextSplit;
                        }
                        else if (_iaNoSplit.AlphaBetaFinished)
                        {
                            _setNextMoveNoSplit();
                            Console.WriteLine("NoSplit finished");
                            _next = _nextNoSplit;
                        }
                        else
                        {
                            Console.WriteLine("None finished");
                        }
                        new MOVTrame(_next).Send(this._socket);
                        break;

                    case "END":
                        this._trame.Receive();
                        if (this._trame.TrameHeader == "BYE")
                        {
                            finish = true;
                        }
                        else
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