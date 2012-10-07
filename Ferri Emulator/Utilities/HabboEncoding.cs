using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Utilities
{
    internal class HabboEncoding
    {
        internal static int DecodeInt32(byte[] v)
        {
            if ((((v[0] | v[1]) | v[2]) | v[3]) < 0)
            {
                return -1;
            }
            return ((((v[0] << 0x18) + (v[1] << 0x10)) + (v[2] << 8)) + v[3]);
        }

        internal static short DecodeInt16(byte[] v)
        {
            if ((v[0] | v[1]) < 0)
            {
                return -1;
            }
            return (short)((v[0] << 8) + v[1]);
        }
    }
}
