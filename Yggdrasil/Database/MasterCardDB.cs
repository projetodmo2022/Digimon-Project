using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{

    public class MasterCardDB
    {
        public static Dictionary<int, MasterCards> cards = new Dictionary<int, MasterCards>();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                if (cards.Count > 0) return;
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        read.Seek(4 + i * 268);
                        MasterCards card = new MasterCards();
                        card.CardID = read.ReadInt();

                        read.Seek(4 + i * 148);
                        card.ItemID = read.ReadInt();
                        read.Seek(4 + i * 246);
                        card.UID = read.ReadUShort();
                        card.U = read.ReadShort();
                        cards.Add(card.CardID, card);
                    }
                }
            }

            SysCons.LogDB("MasterCard.bin", "Loaded {0} cards", cards.Count);
        }

        public static MasterCards Get(int ItemId)
        {
            MasterCards seals = null;
            foreach (KeyValuePair<int, MasterCards> DE in cards)
            {
                if (((int)DE.Key) == ItemId)
                {
                    seals = DE.Value;
                    break;
                }
            }
            return seals;
        }
    }

    public class MasterCards
    {
        public int CardID;
        public ushort UID;
        public short U;
        public int ItemID;
    }

    
}
