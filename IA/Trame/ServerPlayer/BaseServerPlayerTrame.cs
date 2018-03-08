using IA.Trame;
using IA.Trame.ServerPlayer;
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

    class BaseServerPlayerTrame
    {
        private Socket _socket;
        private IDecodable _decoder;

        public string TrameHeader { get; set; }
        public int[, ] TramePayload { get; set; }

        public BaseServerPlayerTrame(Socket socket)
        {
            this._socket = socket;
        }

        public int[,] Receive()
        {
            this._checkTrameType();
            this.TramePayload = this._decoder.Decode(_socket);
            return this.TramePayload;
        }

        public static void CheckTrameType(BaseServerPlayerTrame trame, string headerToCheck)
        {
            if (trame.TrameHeader != headerToCheck)
            {
                throw new NotExpectedTrameException($"Type of expected trame is {headerToCheck} while received {trame.TrameHeader}");
            }
        }

        private void _checkTrameType()
        {
            byte[] buffer = new byte[3];
            while (this._socket.Available < 3) Thread.Sleep(10);

            this._socket.Receive(buffer, 0, 3, SocketFlags.Partial);
            this.TrameHeader = Encoding.ASCII.GetString(buffer, 0, 3);

            switch (this.TrameHeader)
            {
                case "BYE":
                    this._decoder = new BYEDecoder();
                    break;
                case "END":
                    this._decoder = new ENDDecoder();
                    break;
                case "HME":
                    this._decoder = new HMEDecoder();
                    break;
                case "HUM":
                    this._decoder = new HUMDecoder();
                    break;
                case "MAP":
                    this._decoder = new MAPDecoder();
                    break;
                case "SET":
                    this._decoder = new SETDecoder();
                    break;
                case "UPD":
                    this._decoder = new UPDDecoder();
                    break;
                default:
                    break;
            }
        }
    }
}