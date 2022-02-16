using System;
using System.Linq;
using Yggdrasil.Entities;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Yggdrasil.Helpers
{
    [Serializable]
    public class ItemList
    {
        private Item[] items;

        public ItemList(int max)
        {
            items = new Item[max];
            for (int i = 0; i < items.Length; i++)
                items[i] = new Item(0);
        }

        public int Count
        {
            get
            {
                int i = 0;
                for (int j = 0; j < items.Length; j++)
                {
                    if (items[j].ItemId != 0)
                        i++;
                }
                return i;
            }
        }

        public int FindSlot(int itemId)
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i].ItemId == itemId)
                    return i;
            return -1;
        }

        public bool IsFull() => Count >= items.Count();
        public int FindSlot_v2(int ID, int slot)
        {
            int temp = 0;

            for (int i = 0; i < items.Length; i++)
            {

                if (i != slot && items[i].ItemId == ID)
                {
                    temp = i;
                }
                    
                    
            }

            return temp;
        }

        public Item Find(short itemId)
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i].ItemId == itemId)
                    return items[i];
            return null;
        }

        public int EquipSlot(short slotId)
        {
            int slot = 0;

            if ( slotId >= 2000 && slotId <= 2400)
            {
                slot = slotId - 2000;
            }
            if ( slotId >= 1000 && slotId <= 1013)
            {
                slot = slotId - 1000;
            }
            if ( slotId >= 4000 && slotId <= 4008)
            {
                slot = slotId - 4000;
            }
            if ( slotId == 5000 && slotId == 5000)
            {
                slot = slotId - 4986;
            }
            if ( slotId >= 9000 && slotId <= 9014 )
            {
                slot = slotId - 9000;
            }
            return slot;
        }

        /// <summary>
        /// Gets or sets the item at idx
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Item this[int idx]
        {
            get
            {
                return items[idx];
            }
            set
            {
                items[idx] = value;
            }
        }

        /// <summary>
        /// Adds item i to an open slot.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int Add(Item i)
        {
            int slot = GetOpenSlot();
            if (slot == -1)
                return -1;
            else
            {
                items[slot] = i;
                return slot;
            }
        }

        public int Add(Item i, int slot)
        {
            if (slot == -1)
                return -1;
            else
            {
                items[slot] = i;
                return slot;
            }
        }

        public bool Remove(Item i)
        {
            int slot = FindSlot((int)i.ItemId);
            if (slot != -1)
            {
                items[slot] = new Item();
                return true;
            }
            else
                return false;
        }

        public bool Remove(int Slot)
        {
            if (Slot != -1)
            {
                items[Slot] = new Item();
                return true;
            }
            else
                return false;
        }

        public bool RemoveFromCashWareHouse(int Slot, int MaxSlots)
        {
            if (Slot != -1)
            {
                for (int i = Slot; i < MaxSlots; i++)
                {
                    if (i == MaxSlots)
                    {
                        items[i] = new Item();
                    }
                    else
                    {
                        items[i] = items[i + 1];
                    }
                }
                return true;
            }
            else
                return false;
        }

        public bool Contains(int itemId)
        {
            if (FindSlot(itemId) == -1)
                return false;
            return true;
        }

        public int Count_times(int ID)
        {
            int temp = 0;

            for (int i = 0; i < items.Length; i++)
            {
                if(items[i].ItemId==ID)
                    temp++;

            }
            return temp;
        }

        public bool Contains(Item i)
        {
            return Contains((int)i.ItemId);
        }

        private int GetOpenSlot()
        {
            for (int i = 0; i < items.Length; i++)
                if (items[i].ItemId == 0)
                    return i;
            return -1;
        }

        public int GetOpenSlot(int Slot)
        {
            for (int i = 0; i < items.Length; i++)
                if (items[Slot].ItemId == 0)
                    return i;
            return -1;
        }

        public byte[] Serialize()
        {
            byte[] b = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(m, this);
                b = m.ToArray();
            }
            return b;
        }

        public static ItemList Deserialize(byte[] buffer)
        {
            ItemList itemList = null;
            using (MemoryStream m = new MemoryStream(buffer))
            {
                BinaryFormatter f = new BinaryFormatter();
                itemList = (ItemList)f.Deserialize(m);
            }
            return itemList;
        }

        public byte[] LoadCashWH()
        {
            byte[] buffer = null;

            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < items.Length; i++)
                {
                    m.Write(items[i].loadCashWH(),0,68);
                }

                buffer = m.ToArray();
            }

            return buffer;
        }

        public byte[] ToArray()
        {
            byte[] buffer = null;
            using (MemoryStream m = new MemoryStream())
            {
                for (int i = 0; i < items.Length; i++)
                {
                    m.Write(items[i].ToArray(), 0, 68);
                }
                
                //Console.WriteLine(items.Length);
                buffer = m.ToArray();
                //Console.WriteLine(buffer);
            }
            return buffer;
        }
    }
}
