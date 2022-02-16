using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Yggdrasil.Entities;

namespace Yggdrasil.Helpers
{
    [Serializable]
    public class FriendList
    {
        private Friend[] list;
        private static BinaryFormatter f = new BinaryFormatter();

        public FriendList(int max)
        {
            list = new Friend[max];
            for (int i = 0; i < list.Length; i++)
                list[i] = new Friend();
        }


        public int Count
        {
            get
            {
                int i = 0;
                for (int j = 0; j < list.Length; j++)
                {
                    if (list[j].FriendName != null)
                        i++;
                }
                return i;
            }
        }

        public int Add(Friend f)
        {
            int slot = GetOpenSlot();
            if (slot == -1)
                return -1;
            else
            {
                list[slot] = f;
                return slot;
            }
        }

        public bool Remove(Friend f)
        {
            int slot = FindSlot(BitConverter.ToString(f.FriendName));
            if (slot != -1)
            {
                list[slot] = new Friend();
                return true;
            }
            else
                return false;
        }

        public bool Remove(int Slot)
        {
            if (Slot != -1)
            {
                list[Slot] = new Friend();
                return true;
            }
            else
                return false;
        }

        public bool Contains(string name)
        {
            if (FindSlot(name) == -1)
                return false;
            return true;
        }

        public int Count_times(string name)
        {
            int temp = 0;

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].FriendName == Encoding.ASCII.GetBytes(name))
                    temp++;

            }
            return temp;
        }

        public int FindSlot(string name)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i].FriendName == Encoding.ASCII.GetBytes(name))
                    return i;
            return -1;
        }

        public bool Contains(Friend f)
        {
            return Contains(BitConverter.ToString(f.FriendName));
        }

        private int GetOpenSlot()
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i].FriendName == null)
                    return i;
            return -1;
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

        public static FriendList Deserialize(byte[] buffer)
        {
            FriendList list = new FriendList(100);
            using (MemoryStream m = new MemoryStream(buffer))
            {
                list = (FriendList)f.Deserialize(m);
            }
            return list;
        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < 100; i++)
                {
                    m.Write(list[i].ToArray(), 0, list[i].FriendName.Length + 4);
                }

                buffer = m.ToArray();
            }
            return buffer;
        }
    }
}
