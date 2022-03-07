using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class TacticsDB
    {
        public static Dictionary<int, TDBTactic> Tactics = new Dictionary<int, TDBTactic>();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int c = read.ReadInt();
                    for (int i = 0; i < c; i++)
                    {
                        TDBTactic t = new TDBTactic();
                        t.ItemId = read.ReadInt();
                        //Console.WriteLine(t.ItemId);
                        t.Species = read.ReadInt();
                        //Console.WriteLine(t.Species);
                        t.uInt1 = read.ReadInt();
                        //Console.WriteLine(t.uInt1);
                        t.uInt2 = read.ReadInt();
                        //Console.WriteLine(t.uInt2);

                        //DATA INPUT AMOUNT
                        t.Data = read.ReadShort();
                        //Console.WriteLine(t.Data);
                        read.ReadShort();
                        t.uInt3 = read.ReadShort();
                        //Console.WriteLine(t.uInt3);
                        t.uInt4 = read.ReadShort();
                        //Console.WriteLine(t.uInt4);
                        //read.ReadShort();


                        //Console.WriteLine("-------------------------");

                        Tactics.Add(t.ItemId, t);
                    }
                    
                    //More data at the end. Digimon names and descriptions.
                }
            }
            SysCons.LogDB("Tactics.bin", "Loaded {0} entries.", Tactics.Count);
        }


        public static TDBTactic Get(int ItemId)
        {
            TDBTactic tactic = null;
            foreach (KeyValuePair<int, TDBTactic> DE in Tactics)
            {
                if (DE.Key == ItemId)
                {
                    tactic = DE.Value;
                    break;
                }
            }
            return tactic;
        }
    }

    public class TDBTactic
    {
        public int ItemId;
        public int Species;
        public int uInt1;
        public int uInt2;
        public short Data;
        public short uInt3, uInt4;
    }
}
