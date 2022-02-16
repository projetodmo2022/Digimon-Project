using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Yggdrasil;
using Digital_World;

namespace Yggdrasil.Database
{
    public class MapRegionDB
    {
        public static List<Region> RegionList = new List<Region>();

        public static void Load(string fileName)
        {
            if (RegionList.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();

                    for (int i = 0; i < count; i++)
                    {
                        int Count = read.ReadInt();
                        for (int h = 0; h < Count; h++)
                        {
                            Region Regi = new Region();
                            read.Seek(i * 604);
                            Regi.Order = h;
                            Regi.MapId = read.ReadInt();
                            Regi.X = read.ReadInt();
                            Regi.Y = read.ReadInt();
                            RegionList.Add(Regi);
                        }
                    }
                }
            }
            SysCons.LogDB("MapRegion.bin", "Loaded {0} Regions", RegionList.Count);
        }

        public static Region GetID(int ID, int ID2)
        {
            Region item = null;
            foreach (Region _item in RegionList)
                if (_item.MapId == ID && _item.Order == ID2)
                {
                    item = _item;
                    break;
                }
            return item;
        }

    }
    public class Region
    {
        public int Order;
        public int MapId;
        public int X;
        public int Y;
    }
}
