using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{

    public class MapNpcDB
    {
        public static Dictionary<int, MapNPCs> npcs = new Dictionary<int, MapNPCs>();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                if (npcs.Count > 0) return;
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        MapNPCs npc = new MapNPCs();
                        npc.NpcID = read.ReadInt();
                        npc.MapID = read.ReadInt();
                        npc.X = read.ReadInt();
                        npc.Y = read.ReadInt();
                        short u1 = read.ReadShort();
                        short u2 = read.ReadShort();
                        npcs.Add(npc.NpcID, npc);
                    }
                }
            }

            SysCons.LogDB("MapNPC.bin", "Loaded {0} NPCs", npcs.Count);
        }
    }

    public class MapNPCs
    {
        public int MapID;
        public int NpcID;
        public int X, Y;
    }

    
}
