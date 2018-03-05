using IA.Trame;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IA
{
    class Client
    {
        private string _host;
        private int _port;

        private Socket _socket;

        private string _playerName = "EdwardBellaJacob";
        
        public Client(string host, int port)
        {
            this._host = host;
            this._port = port;

            this._socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            this._socket.Connect(this._host, this._port);

            new NMETrame(this._playerName).Send(this._socket);

            new SETTrame().Receive(this._socket);
            new HUMTrame().Receive(this._socket);
            int[,] startPos = new HMETrame().Receive(this._socket);
            new MAPTrame().Receive(this._socket);
            new UPDTrame().Receive(this._socket);

            int[, ] nextPos = new int[2, 1];
            nextPos[0, 0] = startPos[0, 0];
            nextPos[1, 0] = startPos[1, 0] - 1;

            int[,] move = { 
                { startPos[0, 0], startPos[1, 0], 4, nextPos[0, 0], nextPos[1, 0] }
            };

            new MOVTrame(move).Send(this._socket);

            while (true)
            {
                new UPDTrame().Receive(this._socket);
                Thread.Sleep(1000);
          
                startPos = (int[, ]) nextPos.Clone();
                nextPos[0, 0] = nextPos[0, 0] - 1;
                nextPos[1, 0] = nextPos[1, 0];

                int[,] next = {
                    { startPos[0, 0], startPos[1, 0], 4, nextPos[0, 0], nextPos[1, 0] }
                };

                new MOVTrame(next).Send(this._socket);
            }
        }
    }
}
