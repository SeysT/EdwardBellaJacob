using IA.Trame.ServerPlayer;
using System.Net.Sockets;
using System.Threading;

namespace IA.Trame
{
    class UPDDecoder : IDecodable
    {
        public int[,] Decode(Socket socket)
        {
            byte[] buffer = new byte[1];

            while (socket.Available < 1) Thread.Sleep(10);
            socket.Receive(buffer, 0, 1, SocketFlags.Partial);

            int caseNumber = (int) buffer[0];
            int[,] caseUpdates = new int[caseNumber, 5];

            buffer = new byte[5];
            for (int i = 0; i < caseNumber; i++)
            {
                while (socket.Available < 5) Thread.Sleep(10);
                socket.Receive(buffer, 0, 5, SocketFlags.Partial);

                caseUpdates[i, 0] = (int) buffer[0];
                caseUpdates[i, 1] = (int) buffer[1];
                caseUpdates[i, 2] = (int) buffer[2];
                caseUpdates[i, 3] = (int) buffer[3];
                caseUpdates[i, 4] = (int) buffer[4];
            }

            return caseUpdates;
        }
    }
}
