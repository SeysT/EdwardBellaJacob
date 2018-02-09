using System.Net.Sockets;

namespace IA.Trame
{
    class BYETrame : BaseServeurPlayerTrame
    {
        public BYETrame() : base()
        {
            this._trameHeader = "BYE";
        }

        protected override int[,] _decodeTrame(Socket socket)
        {
            return new int[,] { };
        }
    }
}
