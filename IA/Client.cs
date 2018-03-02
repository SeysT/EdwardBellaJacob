using IA.Trame;
using System;
using System.Net.Sockets;
using System.Text;

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
            new HMETrame().Receive(this._socket);
            new MAPTrame().Receive(this._socket);
            new UPDTrame().Receive(this._socket);

            while (true)
            {
                new UPDTrame().Receive(this._socket);
            }
        }
    }
}
