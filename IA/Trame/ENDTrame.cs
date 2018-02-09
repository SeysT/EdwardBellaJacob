using System.Net.Sockets;

namespace IA.Trame
{
    class ENDTrame : BaseServeurPlayerTrame
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
