using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    class PlayerServerFrameMove : PlayerServerFrame // GetTrame() returns Trame
    {
        public PlayerServerFrameMove(int[] intPayload)
        {
            SetHeader(HeaderPlayer.MOV);
            SetPayload(intPayload);
            SetSize();
        }

        public PlayerServerFrameMove(byte[] bytePayload)
        {
            SetHeader(HeaderPlayer.MOV);
            b_payload = bytePayload;
            SetSize();
        }

        protected override void SetPayload(int[] intPayload)
        {
            //TODO optimize this method
            byte[] b_payload = new byte[intPayload.Length*8];
            for (int i = 0; i < intPayload.Length; i++)
            {
                byte[] currentByte = new byte[] { (byte)intPayload[i] };
                currentByte.CopyTo(b_payload, i*8);
            }
        }

        protected override void SetSize()
        {
            if (b_payload.Length % 5 != 0)
            {
                throw new Exception("[PlayerServerFrameMove] b_payload.Length isn't dividable by 5");
            }
            else
            {
                double movementNumber = (b_payload.Length) / 5;
                b_size = BitConverter.GetBytes((int)movementNumber);
            }
        }
    }
}
