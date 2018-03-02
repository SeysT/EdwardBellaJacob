using System.Net.Sockets;
using System.Threading;

namespace IA.Trame
{
    class HUMTrame : BaseServerPlayerTrame
    {
        public HUMTrame() : base()
        {
            this._trameHeader = "HUM";
        }

        protected override int[,] _decodeTrame(Socket socket)
        {
            byte[] buffer = new byte[1];

            while (socket.Available < 1) Thread.Sleep(10);
            socket.Receive(buffer, 0, 1, SocketFlags.Partial);

            int houseNumber = (int) buffer[0];
            int[,] houseCoordinates = new int[houseNumber, 2];

            buffer = new byte[2];
            for (int i = 0; i < houseNumber; i++)
            {
                while (socket.Available < 2) Thread.Sleep(10);
                socket.Receive(buffer, 0, 2, SocketFlags.Partial);

                houseCoordinates[i, 0] = (int) buffer[0];
                houseCoordinates[i, 1] = (int) buffer[1];
            }

            return houseCoordinates;
        }
    }
}
