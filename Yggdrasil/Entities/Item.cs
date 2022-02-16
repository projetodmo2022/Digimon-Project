using System;
using System.IO;
using Yggdrasil.Database;

namespace Yggdrasil.Entities
{
    [Serializable]
    public class Item
    {
        private static Random r = new Random();
        public uint Handle = 0;

        public int ItemId = 0;
        public int Amount = 0;
        public short Unknown = 0;
        public short Unknown1 = 1;
        public short[] Attributes = new short[4];
        public byte DigitaryPower = 0;
        public short Unknown2 = 0;
        public short Unknown3 = 0;
        public byte DigiablePowerRenewelNumber = 0;
        public short Unknown5 = 0;
        public short Stat1 = 0;
        public short Stat2 = 0;
        public short Stat3 = 0;
        public short Stat4 = 0;
        public short Unknown10 = 0;
        public short Unknown11 = 0;
        public short Unknown12 = 0;
        public short Unknown13 = 0;
        public short Stat1_Value = 0;
        public short Stat2_Value = 0;
        public short Stat3_Value = 0;
        public short Stat4_Value = 0;
        public short Unknown18 = 0;
        public short Unknown19 = 0;
        public short Unknown20 = 0;
        public short Unknown21 = 0;
        public short Unknown22 = 0;
        public int time_t = 0;


        public Item()
        {
        }

        public Item(int itemId)
        {
            byte[] b = new byte[4];
            r.NextBytes(b);
            Handle = BitConverter.ToUInt32(b, 0);
            ItemId = itemId;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Item))
            {
                Item o = (Item)obj;
                return o.Handle == this.Handle;
            }
            else
                return base.Equals(obj);
        }

        public static bool operator ==(Item i1, Item i2)
        {
            return i1.Equals(i2);
        }

        public static bool operator !=(Item i1, Item i2)
        {
            return i1.Equals(i2);
        }

        public byte[] loadCashWH()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {

                if (ItemId != 0)
                {
                    m.Write(BitConverter.GetBytes(ItemId), 0, 4);
                    m.Write(BitConverter.GetBytes(Amount), 0, 4);
                    for (int i = 0; i < 52; i++)
                    {
                        m.WriteByte(0);
                    }
                    m.Write(BitConverter.GetBytes(time_t), 0, 4);
                    m.Write(BitConverter.GetBytes(0), 0, 4);

                }
                else
                {
                    for (int i = 0; i < 68; i++)
                    {
                        m.WriteByte(0);
                    }
                }

                buffer = m.ToArray();
            }

            return buffer;
        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[0];
            using (MemoryStream m = new MemoryStream())
            {
                if (ItemId != 0)
                {
                    m.Write(BitConverter.GetBytes(ItemId), 0, 4);
                    m.Write(BitConverter.GetBytes(Amount), 0, 4);
                    m.Write(BitConverter.GetBytes(Unknown2), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown3), 0, 2);
                    m.Write(BitConverter.GetBytes(DigitaryPower), 0, 1);
                    m.Write(BitConverter.GetBytes(DigiablePowerRenewelNumber), 0, 1);
                    m.Write(BitConverter.GetBytes(Unknown1), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown5), 0, 2);
                    m.Write(BitConverter.GetBytes(Attributes[0]), 0, 2);
                    m.Write(BitConverter.GetBytes(Attributes[1]), 0, 2);
                    m.Write(BitConverter.GetBytes(Attributes[2]), 0, 2);
                    m.Write(BitConverter.GetBytes(Attributes[3]), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat1), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat2), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat3), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat4), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown10), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown11), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown12), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown13), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat1_Value), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat2_Value), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat3_Value), 0, 2);
                    m.Write(BitConverter.GetBytes(Stat4_Value), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown18), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown19), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown20), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown21), 0, 2);
                    m.Write(BitConverter.GetBytes(Unknown22), 0, 2);
                    m.Write(BitConverter.GetBytes(time_t), 0, 4);
                    m.Write(BitConverter.GetBytes(0), 0, 4);
                }
                else
                {
                    for (int i = 0; i < 68; i++)
                    {
                        m.WriteByte(0);
                    }
                }
                buffer = m.ToArray();
            }
            return buffer;
        }

        /// <summary>
        /// Full ID of the item
        /// </summary>


        /// <summary>
        /// Property linked to item database
        /// </summary>
        public ItemData ItemData
        {
            get
            {
                ItemData data = ItemListDB.GetItem(this.ItemId);
                return data;
            }
        }
        public int Buy
        {
            get { return ItemData.Buy; }
        }

        public int Sell
        {
            get { return ItemData.Sell; }
        }
        /// <summary>
        /// Maximum amount allowed per inventory slot
        /// </summary>
        public int Max
        {
            get
            {
                return ItemData.Stack;
            }
        }
    }
}
