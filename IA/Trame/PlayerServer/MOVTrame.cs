using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    class MOVTrame : BasePlayerServerTrame // GetTrame() returns Trame
    {
        public MOVTrame(int[,] intPayload)
        {
            SetHeader(HeaderPlayer.MOV);
            SetPayload(intPayload);
            SetSize();
        }

        public MOVTrame(byte[] bytePayload)
        {
            SetHeader(HeaderPlayer.MOV);
            b_payload = bytePayload;
            SetSize();
        }

        protected override void SetPayload(int[,] intPayload)
        {
            b_payload = new byte[intPayload.GetLength(0) * 5];
            for (int i = 0; i < intPayload.GetLength(0); i++)
            {
                for (int j = 0; j < intPayload.GetLength(1); j++)
                {
                    b_payload[i * 5 + j] = (byte)intPayload[i, j];
                }
            }
        }

        protected override void SetSize()
        {
            if (b_payload.Length % 5 != 0)
            {
                throw new Exception("[MOVTrame] b_payload.Length isn't dividable by 5");
            }
            else
            {
                b_size = new byte[] { (byte) (b_payload.Length / 5)};
            }
        }
    }
}
