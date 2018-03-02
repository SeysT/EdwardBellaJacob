using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    enum HeaderPlayer
    {
        NME,
        MOV
    };

    class BasePlayerServerTrame // GetTrame() returns Trame
    {
        protected byte[] b_header; // length 3
        protected byte[] b_size; // 1 Byte int
        protected byte[] b_payload;

        protected void SetHeader(HeaderPlayer h)
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

        protected virtual void SetSize() { }

        protected virtual void SetPayload(string s) { } //for NME

        protected virtual void SetPayload(int[] i) { } //for MOV

        private byte[] GetTrame()
        {
            byte[] b_trame = new byte[b_header.Length + b_payload.Length + b_size.Length];
            b_header.CopyTo(b_trame, 0);
            b_size.CopyTo(b_trame, b_header.Length);
            b_payload.CopyTo(b_trame, b_header.Length + b_size.Length);
            return b_trame;
        }

        public void Send(Socket socket)
        {
            socket.Send(this.GetTrame());
        }
    }
}
