using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{

    public class ExtraExchangeDB
    {
        public static Dictionary<int, ExtraExchanges> exchanges = new Dictionary<int, ExtraExchanges>();

        public static void Load(string fileName)
        {
            using (Stream s = File.OpenRead(fileName))
            {
                if (File.Exists(fileName) == false) return;
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        int NPC_ID = read.ReadInt();
                        int cCount = read.ReadInt();
                        for(int g = 0; g< cCount; g++)
                        {
                            short ID = read.ReadShort();
                            int cCount2 = read.ReadInt();
                            for(int u =0; u<cCount2; u++)
                            {
                                ExtraExchanges exchange = new ExtraExchanges();
                                exchange.DigimonID = read.ReadInt();
                                read.ReadShort();
                                exchange.RequiredLevel = read.ReadInt();
                                exchange.Price = read.ReadInt();
                                read.ReadShort();
                                exchange.ItemCount = read.ReadInt();
                                exchange.requireditems = new int[][] { new int[exchange.ItemCount], new int[exchange.ItemCount] };
                                for(int a = 0; a < exchange.ItemCount; a++)
                                {
                                    exchange.requireditems[0][a] = read.ReadInt();
                                    exchange.requireditems[1][a] = read.ReadInt();
                                }
                                exchange.MaterialCount = read.ReadInt();
                                exchange.submaterials = new int[][] { new int[exchange.MaterialCount], new int[exchange.MaterialCount] };
                                for(int q = 0; q < exchange.MaterialCount; q++)
                                {
                                    exchange.submaterials[0][q] = read.ReadInt();
                                    exchange.submaterials[1][q] = read.ReadInt();
                                }

                                exchanges.Add(exchange.DigimonID, exchange);
                            }
                        }
                    }
                }
            }

            SysCons.LogDB("ExtraExchange.bin", "Loaded {0} ", exchanges.Count);
        }

        public static ExtraExchanges GetExchange(int DigimonID)
        {
            ExtraExchanges iData = null;
            foreach (KeyValuePair<int, ExtraExchanges> kvp in exchanges)
            {
                if (kvp.Value.DigimonID == DigimonID)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }
    }

    public class ExtraExchanges
    {
        public int DigimonID;
        public int RequiredLevel;
        public int Price;
        public int ItemCount;
        public int MaterialCount;
        public int[][] requireditems;
        public int[][] submaterials;
        public int NpcID;
        public int X, Y;
    }

    
}
