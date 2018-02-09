using System.Net.Sockets;
using System.Text;

namespace IA
{
    class Client
    {
        private string _host;
        private int _port;

        private Socket _socket;

        private byte[] _playerName = Encoding.ASCII.GetBytes("EdwardBellaJacob");
        
        public Client(string host, int port)
        {
            this._host = host;
            this._port = port;

            this._socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            this._socket.Connect(this._host, this._port);
        }
    }
}
