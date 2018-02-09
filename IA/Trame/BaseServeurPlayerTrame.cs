using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IA
{
    public class NotExpectedTrameException : Exception
    {
        public NotExpectedTrameException(string message): base(message) { }
    }

    abstract class BaseServeurPlayerTrame
    {
        protected string _trameHeader;

        public BaseServeurPlayerTrame()
        {
            this._trameHeader = "";
        }

        public int[,] Receive(Socket socket)
        {
            this._checkTrameType(socket);
            return this._decodeTrame(socket);
        }

        protected abstract int[,] _decodeTrame(Socket socket);

        protected void _checkTrameType(Socket socket)
        {
            byte[] buffer = new byte[3];
            while (socket.Available < 3) Thread.Sleep(10);

            socket.Receive(buffer, 0, 3, SocketFlags.Partial);
            string trameType = Encoding.ASCII.GetString(buffer, 0, 3);

            if (trameType != this._trameHeader)
            {
                throw new NotExpectedTrameException($"Type of expected trame is {trameType} while received {this._trameHeader}");
            }
        }
    }
}