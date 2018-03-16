﻿using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System;
using IA.Rules;
using System.Collections.Generic;

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

        private int[,] _startPos;
        private Board _board;
        private int _ourIndex;
        private int _theirIndex;

        public Client(string host, int port)
        {
            this._host = host;
            this._port = port;

            this._socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this._trame = new ServerPlayerTrame(this._socket);

            this._sw = new Stopwatch();
        }

        private void _setIndex(Coord hmeCoord, int[,] mapInfos)
        {
            for (int i = 0; i < mapInfos.GetLength(0); i++)
            {
                Coord currentCoord = new Coord(mapInfos[i, 0], mapInfos[i, 1]);
                if (hmeCoord.Equals(currentCoord))
                {
                    if (mapInfos[i, 3] != 0)
                    {
                        this._ourIndex = 3;
                        this._theirIndex = 4;
                    } else
                    {
                        this._ourIndex = 4;
                        this._theirIndex = 3;
                    }

                    return;
                }
            }
        }

        private void _initGame() {
            // First trame has already been received outside _initGame()
            int[,] boardSize = this._trame.TramePayload;
            ServerPlayerTrame.CheckTrameType(this._trame, "SET");

            // We don't need human coordinates to init game
            this._trame.Receive();
            ServerPlayerTrame.CheckTrameType(this._trame, "HUM");

            // We get our starting coordinates
            Coord hmeCoords = new Coord(this._startPos = this._trame.Receive());
            ServerPlayerTrame.CheckTrameType(this._trame, "HME");

            // We get all information from map
            int[,] mapInfos = this._trame.Receive();
            ServerPlayerTrame.CheckTrameType(this._trame, "MAP");

            // We keep in memory what is our specy
            this._setIndex(hmeCoords, mapInfos);

            // We init the game
            Grid grid = new Grid(this._ourIndex, this._theirIndex, mapInfos);
            this._board = new Board(grid, boardSize[0, 0], boardSize[1, 0]);
        }

        private int[, ] _chooseMove()
        {
            // TODO
            // root = new Node(new NodeData())
            // moveCandidates = board.GetMoves(Type.US)
            // value = new MinMax().alphaBeta(root, 7, float.MinValue, float.MaxValue, Type.US, true, moveCandidates, board)
            // moves = MinMax.GetNextMove(root, value)
            Node root = new Node(new NodeData());
            List<Move> moveCandidates = this._board.GetPossibleMoves();
            float value = new MinMax().AlphaBeta(root, 21, float.MinValue, float.MaxValue, true, moveCandidates, this._board);
            List<Move> moves = MinMax.GetNextMove(root, value);

            int[, ] toReturn = new int[0, 0];
            return toReturn;
        }

        private void _updateGame()
        {
            // TODO
            int[,] updates = this._trame.TramePayload;
            Thread.Sleep(1000);
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