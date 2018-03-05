using IA.Trame.ServerPlayer;
using System.Net.Sockets;

namespace IA.Trame
{
    class BYEDecoder : IDecodable
    {
        public int[,] Decode(Socket socket)
        {
            return new int[,] { };
        }
    }
}
