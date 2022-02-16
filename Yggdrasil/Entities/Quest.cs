using System;
using System.IO;

namespace Yggdrasil.Entities
{
    [Serializable]
    public class Quest
    {
        public short QuestId = 2911;
        public byte Goal1 = 0;
        public byte Goal2 = 0;
        public byte Goal3 = 0;
        public byte Goal4 = 0;
        public byte Goal5 = 0;

        public Quest() { }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {

                m.Write(BitConverter.GetBytes(QuestId), 0, 2);
                m.WriteByte(Goal1);
                m.WriteByte(Goal2);
                m.WriteByte(Goal3);
                m.WriteByte(Goal4);
                m.WriteByte(Goal5);

                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
