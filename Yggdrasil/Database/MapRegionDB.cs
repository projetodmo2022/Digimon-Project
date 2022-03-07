using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class MapRegionDB
    {
        public static List<Region> Region = new List<Region>();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
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
                            
                            Regi.MapId = read.ReadInt();
                            Regi.s_nCenterX = read.ReadInt();
                            Regi.s_nCenterY = read.ReadInt();
                            Regi.s_nRadius = read.ReadInt();
                            Regi.s_szDiscript = read.ReadZString(Encoding.Unicode, 128 * 2);
                            Regi.s_szDiscript_Eng = read.ReadZString(Encoding.Unicode, 128 * 2);
                            Regi.s_cBGSound = read.ReadZString(Encoding.Unicode, 64);
                            Regi.s_nFatigue_Type = read.ReadUShort();
                            Regi.s_nFatigue_DeBuff = read.ReadUShort();
                            Regi.s_nFatigue_DeBuff = read.ReadUShort();
                            Regi.s_nFatigue_StartTime = read.ReadUShort();
                            Regi.s_nFatigue_AddTime = read.ReadUShort();
                            Regi.s_nFatigue_AddPoint = read.ReadInt();
                            Regi.Order = h;
                            
                            
                            Region.Add(Regi);
                        }
                    }
                }
            }
            SysCons.LogDB("MapRegion.bin", "Loaded {0} Regions", Region.Count);
        }

        public static Region GetID(int ID, int ID2)
        {
            Region item = null;
            foreach (Region _item in Region)
                if (_item.MapId == ID && _item.Order == ID2)
                {
                    item = _item;
                    break;
                }
            return item;
        }

        public static Region GetById(int Id)
        {
            return Region.FirstOrDefault(x=>x.MapId == Id);
        }

    }
    public class Region


    {
        public int MapId;
        public int s_nCenterX;
        public int s_nCenterY;
        public int s_nRadius;
        public string s_szDiscript;
        public string s_szDiscript_Eng;
        public string s_cBGSound;
        public ushort s_nFatigue_Type;
        public ushort s_nFatigue_DeBuff;
        public ushort s_nFatigue_StartTime;
        public ushort s_nFatigue_AddTime;
        public int s_nFatigue_AddPoint;
        public int Order;
        
        
    }
}
