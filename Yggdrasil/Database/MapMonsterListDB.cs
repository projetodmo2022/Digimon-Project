using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Yggdrasil;
using Digital_World;
using Yggdrasil.Entities;


namespace Yggdrasil.Database
{

    public class MapMonsterListDB
    {
        public static Dictionary<int, MapMonsters> Mapmonsters = new Dictionary<int, MapMonsters>();

        public static void Load(string fileName, List<MonsterEntity> MonstersEntity)
        {
            using (Stream s = File.OpenRead(fileName))
            {
                if (Mapmonsters.Count > 0) return;
                using (BitReader read = new BitReader(s))
                {
                    Random rand = new Random();
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        MapMonsters monsters = new MapMonsters();
                        monsters.MonsterID = read.ReadInt();
                        monsters.Count = read.ReadInt();
                        for (int g = 0; g < monsters.Count; g++)
                        {
                            monsters.monster = new List<Monster>(monsters.Count);
                            Monster monster = new Monster();
                            monster.MapID = new int[monsters.Count];
                            monster.MonsterCount = new int[monsters.Count];
                            monster.MapID[g] = read.ReadInt();
                            monster.MonsterCount[g] = read.ReadInt();
                            monster.MonsterSpecies = new int[monster.MonsterCount[g]];
                            monster.Position = new int[][] { new int[monster.MonsterCount[g]], new int[monster.MonsterCount[g]] };
                            for (int u = 0; u < monster.MonsterCount[g]; u++)
                            {
                                int MapID = read.ReadInt();
                                MonsterEntity entity = new MonsterEntity();
                                monster.MonsterSpecies[u] = read.ReadInt();
                                monster.Position[0][u] = read.ReadInt();
                                monster.Position[1][u] = read.ReadInt();
                                entity.Species = monster.MonsterSpecies[u];
                                entity.Location.Map = MapID;
                                entity.Location.PosX = monster.Position[0][u];
                                entity.Location.PosY = monster.Position[1][u];
                                entity.Handle = 64999 + i + rand.Next(1, 5000);
                                entity.Collision = read.ReadInt();
                                LoadStats(entity);
                                read.ReadInt();
                                read.ReadInt();
                                read.ReadInt();
                                read.ReadInt();
                                read.ReadInt();
                                read.ReadInt();
                                read.ReadInt();
                                MonstersEntity.Add(entity);
                            }
                            monsters.monster.Add(monster);
                        }
                        Mapmonsters.Add(monsters.MonsterID, monsters);
                    }
                }
            }

            SysCons.LogDB("MapMonsterList.bin", "Loaded {0} monsters", Mapmonsters.Count);
            SysCons.LogDB("Monster.bin", "Loaded {0} monsters", MonstersEntity.Count);
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
            foreach (KeyValuePair<int, MapMonsters> kvp in Mapmonsters)
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
        public int MonsterID;
        public int Count;
        public List<Monster> monster;
    }
    public class Monster
    {
        public int[] MapID;
        public int[] MonsterSpecies;
        public int MonsterID;
        public int[] MonsterCount;
        public int[][] Position;
    }
}