using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA
{
enum HeaderPlayerIA
    {
        NME,
        MOV
    }

    class PlayerServerTrame
    {
        string s_header; // length 3
        string s_size; // 1 Byte int
        string s_payload; 
    }
}
