using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;

using Yggdrasil.Entities;


namespace Yggdrasil.Database
{

    public class MapMonsterListDB
    {
        public static Dictionary<int, MapMonsters> MapMonsters = new Dictionary<int, MapMonsters>();

        public static void Load(string fileName, List<MonsterEntity> MonstersEntity)
        {
            if (File.Exists(fileName) == false) 
                return;

            using (Stream s = File.OpenRead(fileName))
            {
                if (MapMonsters.Count > 0) 
                    return;

                using (BitReader read = new BitReader(s))
                {
                    Random rand = new Random();
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        MapMonsters map = new MapMonsters();
                        map.FileID = read.ReadInt();
                        map.nSize = read.ReadInt();
                        for (int j = 0; j < map.nSize; ++j)
                        {
                            map.Map = read.ReadInt();
                            map.MapNum = read.ReadInt();

                            for (int k = 0; k < map.MapNum; k++)
                            {
                                map.MapID = read.ReadInt();
                                map.MonsterID = read.ReadInt();
                                map.CenterX = read.ReadInt();
                                map.CenterY = read.ReadInt();
                                map.Radius = read.ReadInt();

                                var entity = new MonsterEntity();
                                entity.Species = map.MonsterID;
                                entity.Location = new Position() { Map = map.MapID, PosX = map.CenterX, PosY = map.CenterY };
                                entity.Collision = map.Radius;
                                entity.Handle = 64999 + i + rand.Next(1, 5000);

                                if(!MonstersEntity.Contains(entity))
                                    MonstersEntity.Add(entity);

                                map.Count = read.ReadInt();
                                map.RespawnTime = read.ReadInt();
                                map.KillGenMonFTID = read.ReadInt();
                                map.KillgenCount = read.ReadInt();
                                map.KillgenViewCnt = read.ReadInt();
                                map.MoveType = read.ReadInt();
                                map.InstRespawn = read.ReadByte();
                                map.u10 = read.ReadShort();
                                map.u2 = read.ReadByte();

                            }

                        }
                        if (!MapMonsters.ContainsKey(map.FileID))
                        {
                            MapMonsters.Add(map.FileID, map);
                        }

                        if (!MapMonsters.ContainsKey(map.MonsterID))
                        {
                            MapMonsters.Add(map.MonsterID, map);
                        }
                    }
                }
            }

            SysCons.LogDB("MapMonsterList.bin", "Loaded {0} monsters", MapMonsters.Count);
        }

        private static void LoadStats(MonsterEntity entity)
        {
            var digimon = DigimonListDB.GetDigimon(entity.Species);
            if (digimon != null)
            {
                entity.HP = digimon.HP;
                entity.MaxHP = digimon.HP;
            }
        }

        public static MapMonsters GetMonster(int DigimonID)
        {
            MapMonsters iData = null;
            foreach (KeyValuePair<int, MapMonsters> kvp in MapMonsters)
            {
                if (kvp.Value.MonsterID == DigimonID)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }
    }

    public class MapMonsters
    {
        public int FileID;
        public int nSize;
        public int Map;
        public int MapNum;
        public int MapID;
        public int MonsterID;
        public int CenterX;
        public int CenterY;
        public int Radius;
        public int Count;
        public int RespawnTime;
        public int KillGenMonFTID;
        public int KillgenCount;
        public int KillgenViewCnt;
        public int MoveType;
        public byte InstRespawn;
        public short u10;
        public byte u2;
        
    }
    
}