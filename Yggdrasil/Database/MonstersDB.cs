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

    public class MonstersDB
    {
        public static Dictionary<int, Monsters> monsters = new Dictionary<int, Monsters>();

        public static void Load(string fileName, List<MonsterEntity> MonsterEntity)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                if (monsters.Count > 0) return;
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        Monsters monster = new Monsters();
                        monster.MonsterID = read.ReadInt();
                        monster.ModelDigimon = read.ReadInt();
                        monster.Name = read.ReadZString(Encoding.Unicode, 128);
                        monster.Comment = read.ReadZString(Encoding.Unicode, 68);
                        monster.Title = read.ReadZString(Encoding.Unicode, 128);
                        monster.Level = read.ReadShort();
                        monster.EXP = read.ReadUShort();
                        monster.Battle = read.ReadUShort();
                        monster.Unknown = read.ReadShort();
                        monster.HP = read.ReadInt();
                        monster.DS = read.ReadInt();
                        monster.DE = read.ReadUShort();
                        monster.EV = read.ReadUShort();
                        monster.MS = read.ReadUShort();
                        monster.WS = read.ReadUShort();
                        monster.CT = read.ReadUShort();
                        monster.AT = read.ReadUShort();
                        monster.AS = read.ReadUShort();
                        monster.AR = read.ReadUShort();
                        monster.HT = read.ReadUShort();
                        monster.Sight = read.ReadUShort();
                        monster.HuntRange = read.ReadUShort();
                        monster.Scale = read.ReadFloat();
                        monster.Unknown2 = read.ReadUShort();
                        monster.Class = read.ReadUShort();
                        monster.Icon1 = read.ReadShort();
                        monster.Icon2 = read.ReadShort();
                        monster.Icon3 = read.ReadShort();
                        monster.Icon4 = read.ReadShort();
                        monster.Icon5 = read.ReadShort();
                        monster.Icon6 = read.ReadShort();
                        monster.ExpMin = read.ReadUShort();
                        monster.ExpMax = read.ReadUShort();
                        monster.Unknown3 = read.ReadUShort();


                        for (int p = 0; p < MonsterEntity.Count; p++)
                        {
                            if (MonsterEntity[p].Species == monster.MonsterID)
                            {
                                MonsterEntity[p].Level = monster.Level;
                                MonsterEntity[p].HP = monster.HP;
                                //MonsterEntity[p].DS = monster.DS;


                            }
                        }

                        monsters.Add(monster.MonsterID, monster);
                    }
                }
            }

            SysCons.LogDB("Monsters.bin", "Loaded {0} Monsters", monsters.Count);
        }

        public static Monsters GetMonster(int MonsterID)
        {
            Monsters iData = null;
            foreach (KeyValuePair<int, Monsters> kvp in monsters)
            {
                if (kvp.Value.MonsterID == MonsterID)
                {
                    iData = kvp.Value;
                    break;
                }
            }
            return iData;
        }
    }

    public enum eBattle
    {
        Guarder,    // passive attacker
        Helper,     // party helper	, 190319 - ¸ÁºÎ¼®¿ëÀ¸·Î »ç¿ëÇÏ°Ú´Ù°íÇØ¼­ º¯°æ
        Attacker,   // auto attacker

        MaxBattle,
    };
    public class Monsters
    {
        public int Ht;
        public int Lv;
        public int HP;
        public int DS;
        public uint Activetype;
        public uint TalkID;
        public uint Range;
        public ushort DE;
        public ushort EV;
        public ushort MS;
        public ushort WS;
        public ushort CT;
        public ushort AT;
        public ushort AS;
        public ushort AR;
        public ushort HT;
        public ushort Sight;
        public ushort HuntRange;
        public ushort EXP;
        public ushort Battle;
        public short Unknown;
        public short Level;
        public string Name;
        public string Tag;
        public string Comment;
        public string Title;
        public ushort Unknown2;
        public ushort Class; // monster types
        public short Icon1;
        public short Icon2;
        public short Icon3;
        public short Icon4;
        public short Icon5;
        public short Icon6;
        public ushort ExpMin;
        public ushort ExpMax;
        public ushort Unknown3;
        public int MonsterID;
        public int ModelDigimon;
        public short Skill_Type;
        public short unk;
        public short RangeIDX;
        public short Ani_Delay;
        public short Valocity;
        public short Accel;
        public short Eff_Factor;
        public short IDX;
        public short Direction;
        public short TargetingType;
        public short RefCode;
        public float Scale;
        public short Skill_IDX;
        public short SequenceID;
        public short Eff_Val_Min;
        public short Eff_Val_Max;
        public short CoolTime;
        public short CastTime;
        public short Eff_Fact_Val;



    }


}