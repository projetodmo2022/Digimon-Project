using System;
using System.IO;

namespace Yggdrasil.Entities
{
    [Serializable]
    public class Seal
    {
        public short ID = 0;
        public ushort u1 = 0;
        public short u2 = 0;
        public short Amount = 0;

        public Seal() { }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {

                m.Write(BitConverter.GetBytes(ID), 0, 2);
                m.Write(BitConverter.GetBytes(u1), 0, 2);
                m.Write(BitConverter.GetBytes(u2), 0, 2);
                m.Write(BitConverter.GetBytes(Amount), 0, 2);

                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
