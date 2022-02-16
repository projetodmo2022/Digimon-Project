using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Yggdrasil.Helpers
{
    [Serializable]
    public class QuestList
    {
        private Quest[] list = new Quest[20];
        private static BinaryFormatter f = new BinaryFormatter();

        public QuestList()
        {
            list = new Quest[20];
            for (int i = 0; i < 20; i++)
                list[i] = new Quest();
        }

        private int FindSlot()
        {
            int slot = -1;
            for (int i = 0; i < 20; i++)
            {
                if (list[i].QuestId >= 0)
                {
                    slot = i;
                    break;
                }
            }
            return slot;
        }

        public bool Add(Quest q)
        {
            int slot = FindSlot();
            if (slot == -1) return false;
            list[slot] = q;
            return true;
        }

        public byte[] Serialize()
        {
            byte[] buffer = new byte[0];
            using (
            MemoryStream m = new MemoryStream())
            {
                f.Serialize(m, this);
                buffer = m.ToArray();
            }
            return buffer;
        }

        public static QuestList Deserialize(byte[] buffer)
        {
            QuestList list = new QuestList();
            using (MemoryStream m = new MemoryStream(buffer))
            {
                list = (QuestList)f.Deserialize(m);
            }
            return list;
        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < 20; i++)
                {
                    m.Write(list[i].ToArray(), 0, 7);
                }

                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
