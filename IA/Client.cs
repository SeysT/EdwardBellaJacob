using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
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
        private BaseServerPlayerTrame _trame;

        private int[,] _startPos;

        public Client(string host, int port)
        {
            this._host = host;
            this._port = port;

            this._socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this._trame = new BaseServerPlayerTrame(this._socket);

            this._sw = new Stopwatch();
        }

        private void _initGame() {
            // First trame has already been received outside _initGame()
            BaseServerPlayerTrame.CheckTrameType(this._trame, "SET");

            this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "HUM");

            this._startPos = this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "HME");

            this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "MAP");
        }

        private int[, ] _chooseMove()
        {
            int[,] nextPos = (int[,]) this._startPos.Clone();
            nextPos[0, 0] = nextPos[0, 0] - 1;
            nextPos[1, 0] = nextPos[1, 0];

            int[,] next = { { this._startPos[0, 0], this._startPos[1, 0], 4, nextPos[0, 0], nextPos[1, 0] } };

            this._startPos = nextPos;
            return next;
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
                int[, ] received = this._trame.Receive();

                switch (this._trame.TrameHeader)
                {
                    case "UPD":
                        Thread.Sleep(1000);
                        this._sw.Start();

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