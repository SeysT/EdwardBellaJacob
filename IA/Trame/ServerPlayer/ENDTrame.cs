using System.Net.Sockets;

namespace IA.Trame
{
    class ENDTrame : BaseServerPlayerTrame
    {
        public ENDTrame() : base()
        {
            this._trameHeader = "END";
        }

        protected override int[,] _decodeTrame(Socket socket)
        {
            return new int[,] { };
        }
    }
}
