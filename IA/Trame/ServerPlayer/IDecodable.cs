using System.Net.Sockets;

namespace IA.Trame.ServerPlayer
{
    interface IDecodable
    {
        int[, ] Decode(Socket socket);
    }
}
