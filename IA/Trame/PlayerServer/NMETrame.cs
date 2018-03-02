using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    class NMETrame : BasePlayerServerTrame // GetTrame() returns Trame
    {
        public NMETrame(string playerName)
        {
            SetHeader(HeaderPlayer.NME);
            SetPayload(playerName);
            SetSize();
        }

        protected override void SetPayload(string playerName)
        {
            b_payload = Encoding.ASCII.GetBytes(playerName); //TODO does this work ?
        }

        protected override void SetSize()
        {
            b_size = new byte[] { (byte) (b_payload.Length) };
        }
    }
}
