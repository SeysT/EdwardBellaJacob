using System.Net.Sockets;
using System.Threading;

namespace IA.Trame
{
    class MAPTrame : BaseServerPlayerTrame
    {
        public MAPTrame() : base()
        {
            this._trameHeader = "MAP";
        }

        protected override int[,] _decodeTrame(Socket socket)
        {
            byte[] buffer = new byte[1];

            while (socket.Available < 1) Thread.Sleep(10);
            socket.Receive(buffer, 0, 1, SocketFlags.Partial);

            int caseNumber = (int)buffer[0];
            int[,] caseConfigurations = new int[caseNumber, 2];

            buffer = new byte[5];
            for (int i = 0; i < caseNumber; i++)
            {
                while (socket.Available < 2) Thread.Sleep(10);
                socket.Receive(buffer, 0, 2, SocketFlags.Partial);

                caseConfigurations[i, 0] = (int) buffer[0];
                caseConfigurations[i, 1] = (int) buffer[1];
                caseConfigurations[i, 2] = (int) buffer[2];
                caseConfigurations[i, 3] = (int) buffer[3];
                caseConfigurations[i, 4] = (int) buffer[4];
            }

            return caseConfigurations;
        }
    }
}