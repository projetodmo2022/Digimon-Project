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
    public class SealList
    {
        private Seal[] list;
        private static BinaryFormatter f = new BinaryFormatter();

        public SealList(int max)
        {
            list = new Seal[max];
            for (int i = 0; i < max; i++)
                list[i] = new Seal();
        }

        public Seal this[int idx]
        {
            get
            {
                return list[idx];
            }
            set
            {
                list[idx] = value;
            }
        }

        private int FindSlot()
        {
            int slot = -1;
            for (int i = 0; i < 120; i++)
            {
                if (list[i].ID > 0)
                {
                    slot = i;
                    break;
                }
            }
            return slot;
        }

        public int Count
        {
            get
            {
                int i = 0;
                for (int j = 0; j < list.Length; j++)
                {
                    if (list[j].ID != 0)
                        i++;
                }
                return i;
            }
        }

        public bool Add(Seal q)
        {
            int slot = FindSlot();
            if (slot == -1) return false;
            list[slot] = q;
            return true;
        }

        public bool Remove(Seal i)
        {
            int slot = FindSlot();
            if (slot != -1)
            {
                list[slot] = new Seal();
                return true;
            }
            else
                return false;
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

        public static SealList Deserialize(byte[] buffer)
        {
            SealList sealList = null;
            using (MemoryStream m = new MemoryStream(buffer))
            {
                BinaryFormatter f = new BinaryFormatter();
                sealList = (SealList)f.Deserialize(m);
            }
            return sealList;
        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < 120; i++)
                {
                    m.Write(list[i].ToArray(), 0, 8);
                }

                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
