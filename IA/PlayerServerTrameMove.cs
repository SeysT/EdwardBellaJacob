using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
    class PlayerServerTrameMove : PlayerServerTrame
    {
        public PlayerServerTrameMove(byte[] movementList)
        {
            b_payload = movementList;
        }

        internal override void setSize()
        {
            double movementNumber = (b_payload.Length) / 5;
            b_size = BitConverter.GetBytes((int)movementNumber);
        }
    }
}
