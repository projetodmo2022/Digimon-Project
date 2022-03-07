using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class MapListDB
    {
        public static Dictionary<int, MapData> MapList = new Dictionary<int, MapData>();

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
                        MapData map = new MapData();
                        map.MapID = read.ReadInt();
                        int Msize = read.ReadInt();
                        byte[] mname_get = new byte[Msize];
                        for (int g = 0; g < Msize; g++)
                        {
                            mname_get[g] = read.ReadByte();
                        }
                        map.Name = Encoding.ASCII.GetString(mname_get).Trim();
                        int MapLocSize = read.ReadInt();
                        byte[] mloc_get = new byte[MapLocSize];
                        for (int f = 0; f < MapLocSize; f++)
                        {
                            mloc_get[f] = read.ReadByte();
                        }
                        map.MapLocation = Encoding.ASCII.GetString(mloc_get).Trim();
                        int MapSoundSize = read.ReadInt();

                        byte[] msound_get = new byte[MapSoundSize];
                        for (int z = 0; z < MapSoundSize; z++)
                        {
                            msound_get[z] = read.ReadByte();
                        }
                        map.MapSound = Encoding.ASCII.GetString(msound_get).Trim();

                        map.MapNumber = read.ReadInt();
                        map.MapNumber2 = read.ReadInt();
                        int dnamesize = read.ReadInt();
                        map.DisplayName = read.ReadZString(Encoding.Unicode, dnamesize);
                        read.Seek((int)read.InnerStream.BaseStream.Position - 4);
                        int testsize = read.ReadInt();
                        map.Test = new ushort[testsize];
                        for (int k = 0; k < testsize; k++)
                        {
                            map.Test[k] = read.ReadUShort();
                        }
                        read.ReadShort();
                        read.ReadInt();
                        read.ReadInt();
                        read.ReadShort();
                        read.ReadShort();
                        read.ReadShort();
                        read.ReadInt();
                        /*
                        read.Seek(4 + i * 676);
                        MapData map = new MapData();
                        map.MapID = read.ReadInt();
                        map.MapNumber = read.ReadInt();
                        read.Skip(4);
                        map.Name = read.ReadZString(Encoding.ASCII);
                        read.Skip(676 - (int)(338 + (read.InnerStream.BaseStream.Position - (676 * i)) - 2));
                        map.DisplayName = read.ReadZString(Encoding.Unicode);

                        */

                        //Console.WriteLine($"{map.MapID} | {map.Name} | {map.MapLocation} | {map.MapSound} | {map.DisplayName} | {testsize}");
                        MapList.Add(map.MapID, map);
                    }
                }
            }
            SysCons.LogDB("MapList.bin", "Loaded {0} maps.", MapList.Count);
        }

        public static MapData GetMap(int mapId)
        {
            if (MapList.ContainsKey(mapId))
                return MapList[mapId];
            else
                return null;
        }
    }

    public class MapData
    {
        public ushort[] Test;
        public int MapID;
        public string MapLocation;
        public int MapNumber;
        public int MapNumber2;
        public string MapSound;
        public string Name;
        public string DisplayName;
        public int[] Handlers;

        public override string ToString()
        {
            return string.Format("{1} {0}", DisplayName, MapID);
        }
    }
}
