using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class WorldMapDB
    {
        public static Dictionary<int, WorldMapInfo> WorldMapInfoList = new Dictionary<int, WorldMapInfo>();
        public static Dictionary<int, AreaMapInfo> AreaMapInfoList = new Dictionary<int, AreaMapInfo>();

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
                        WorldMapInfo mapInfo = new WorldMapInfo();
                        mapInfo.s_nID = read.ReadUShort();
                        mapInfo.s_szName = read.ReadZString(Encoding.Unicode, 48 * 2);
                        mapInfo.s_szComment = read.ReadZString(Encoding.Unicode, 256 * 2);
                        mapInfo.s_nWorldType = read.ReadUShort();
                        mapInfo.s_nUI_X = read.ReadUShort();
                        mapInfo.s_nUI_Y = read.ReadUShort();
                        if (!WorldMapInfoList.ContainsKey(mapInfo.s_nID))
                        {
                            WorldMapInfoList.Add(mapInfo.s_nID, mapInfo);
                        }

                    }

                    int count2 = read.ReadInt();
                    for (int j = 0; j < count2; j++)
                    {
                        AreaMapInfo areamapInfo = new AreaMapInfo();
                        areamapInfo.d_nMapID = read.ReadUShort();
                        areamapInfo.d_szName = read.ReadZString(Encoding.Unicode, 48 * 2);
                        areamapInfo.d_szComment = read.ReadZString(Encoding.Unicode, 256 * 2);
                        areamapInfo.d_nAreaType = read.ReadByte();
                        areamapInfo.d_nFieldType = read.ReadByte();
                        areamapInfo.d_nFTDetail = read.ReadInt();
                        areamapInfo.d_nUI_X = read.ReadUShort();
                        areamapInfo.d_nUI_Y = read.ReadUShort();
                        areamapInfo.d_fGaussianBlur = read.ReadFloat();
                        areamapInfo.d_fGaussianBlur = read.ReadFloat();
                        areamapInfo.d_fGaussianBlur = read.ReadFloat();
                        if (!AreaMapInfoList.ContainsKey(areamapInfo.d_nMapID))
                        {
                            AreaMapInfoList.Add(areamapInfo.d_nMapID, areamapInfo);
                        }

                    }
                }
            }
            SysCons.LogDB("WorldMap.bin", "Loaded {0} WorldMap.", WorldMapInfoList.Count);
            SysCons.LogDB("WorldMap.bin", "Loaded {0} AreaMap.", AreaMapInfoList.Count); 
        }
    }

    public class WorldMapInfo
    {

        public ushort s_nID;
        public String s_szName;
        public String s_szComment;
        public ushort s_nWorldType;
        public ushort s_nUI_X;
        public ushort s_nUI_Y;
    }

    public class AreaMapInfo
    {

        public ushort d_nMapID;
        public String d_szName;
        public String d_szComment;
        public byte d_nAreaType;
        public byte d_nFieldType;
        public int d_nFTDetail;
        public ushort d_nUI_X;
        public ushort d_nUI_Y;
        public float d_fGaussianBlur;
    }
    
}

