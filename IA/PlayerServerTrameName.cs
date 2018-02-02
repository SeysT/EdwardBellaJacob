using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    class PlayerServerTrameName : PlayerServerTrame
    {
        public PlayerServerTrameName(string playerName)
        {
            setHeader(HeaderPlayer.NME);
            setPayload(playerName);
            setSize();
        }

        internal override void setSize()
        {
            b_size = new byte[] { (byte) (b_payload.Length) };
        }

        internal override void setPayload(string playerName)
        {
            b_payload = Encoding.ASCII.GetBytes(playerName);
        }
    }
}
