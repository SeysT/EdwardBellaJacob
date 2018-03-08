using IA.Trame.ServerPlayer;
using System.Net.Sockets;
using System.Threading;

namespace IA.Trame
{
    class HMEDecoder : IDecodable
    {
        public int[,] Decode(Socket socket)
        {
            byte[] buffer = new byte[2];

            while (socket.Available < 2) Thread.Sleep(10);
            socket.Receive(buffer, 0, 2, SocketFlags.Partial);

            return new int[,] { { (int)buffer[0] }, { (int)buffer[1] } };
        }
    }
}
