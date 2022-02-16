using System.Runtime.InteropServices;
using System;

namespace Yggdrasil.Helpers
{
    public class Import
    {
        [DllImport("msvcrt.dll",CallingConvention=CallingConvention.Cdecl)]  
        public static extern void srand(uint seed);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rand();

        public static byte[] GetRandBytes()
        {
            uint seed = (uint)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Import.srand(seed);
            byte[] b = new byte[500];
            for (int i = 0; i < b.Length; i++)
                b[i] = (byte)(Import.rand() % 256);
            return b;
        }

        public static byte[] GetRandBytes(uint seed)
        {
            srand(seed);
            byte[] b = new byte[500];
            for (int i = 0; i < b.Length; i++)
                b[i] = (byte)(Import.rand() % 256);
            return b;
        }

        public static byte[] GetRandBytes(uint seed, int count)
        {
            srand(seed);
            byte[] b = new byte[count];
            for (int i = 0; i < b.Length; i++)
                b[i] = (byte)(Import.rand() % 256);
            return b;
        }
    }
}
