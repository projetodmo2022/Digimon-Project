using System;
using System.IO;
using System.Text;

namespace Yggdrasil.Entities
{
    [Serializable]
    public class Friend
    {
        public byte[] FriendName;

        public Friend() { }


        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                if(FriendName != null)
                {
                    m.WriteByte(0x0);
                    m.Write(FriendName, 0, FriendName.Length);
                    m.WriteByte(0x0);
                }

                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
