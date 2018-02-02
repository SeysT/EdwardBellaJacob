using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    enum HeaderPlayer
    {
        NME,
        MOV
    };

    class PlayerServerTrame
    {
        internal byte[] b_size; // 1 Byte int
        internal byte[] b_header; // length 3
        internal byte[] b_payload;

        internal void setHeader(HeaderPlayer h)
        {
            if (h == HeaderPlayer.NME)
            {
                b_header = new byte[3] { (Byte)'N', (Byte)'M', (Byte)'E' };
            }
            else // h == HeaderPlayer.MOV
            {
                b_header = new byte[3] { (Byte)'M', (Byte)'O', (Byte)'V' };
            }
        }

        internal virtual void setPayload(string s) { }

        internal virtual void setPayload(byte[] s)
        {
            b_payload = s;
        }

        internal virtual void setSize()
        {
            b_size = new byte[0];
        }
        
        public byte[] getTrame()
        {
            byte[] b_trame = new byte[b_header.Length + b_payload.Length + b_size.Length];
            b_header.CopyTo(b_trame, 0);
            b_payload.CopyTo(b_trame, b_header.Length);
            b_size.CopyTo(b_trame, b_header.Length + b_payload.Length);
            return b_trame;
        }
    }
}
