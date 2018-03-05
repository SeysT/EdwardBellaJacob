using IA.Trame;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace IA
{
    class Client
    {
        private string _host;
        private int _port;

        private Socket _socket;

        private string _playerName = "EdwardBellaJacob";
        private BaseServerPlayerTrame _trame;

        public Client(string host, int port)
        {
            this._host = host;
            this._port = port;

            this._socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this._trame = new BaseServerPlayerTrame(this._socket);
        }

        public void Start()
        {
            this._socket.Connect(this._host, this._port);


            new NMETrame(this._playerName).Send(this._socket);

            this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "SET");

            this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "HUM");

            int[,] startPos = this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "HME");

            this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "MAP");

            this._trame.Receive();
            BaseServerPlayerTrame.CheckTrameType(this._trame, "UPD");

            int[, ] nextPos = new int[2, 1];
            nextPos[0, 0] = startPos[0, 0];
            nextPos[1, 0] = startPos[1, 0] - 1;

            int[,] move = { { startPos[0, 0], startPos[1, 0], 4, nextPos[0, 0], nextPos[1, 0] } };

            new MOVTrame(move).Send(this._socket);

            while (true)
            {
                this._trame.Receive();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                if (this._trame.TrameHeader != "UPD")
                {
                    Console.WriteLine(this._trame.TrameHeader);
                    this._trame.Receive();
                    Console.WriteLine(this._trame.TrameHeader);
                    break;
                }
                Thread.Sleep(1000);
          
                startPos = (int[, ]) nextPos.Clone();
                nextPos[0, 0] = nextPos[0, 0] - 1;
                nextPos[1, 0] = nextPos[1, 0];

                int[,] next = {
                    { startPos[0, 0], startPos[1, 0], 4, nextPos[0, 0], nextPos[1, 0] }
                };

                sw.Stop();
                Console.WriteLine("Elapsed Time={0}", sw.Elapsed);
                new MOVTrame(next).Send(this._socket);
            }
        }
    }
}
