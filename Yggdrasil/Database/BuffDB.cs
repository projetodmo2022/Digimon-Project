using System;
using System.Collections.Generic;
using System.IO;
using Yggdrasil.Helpers;
using System.Text;
using Digital_World;


namespace Yggdrasil.Database
{
    public class BuffDB
    {
        public static Dictionary<int, BuffData> BuffList = new Dictionary<int, BuffData>();

        public static void Load(string fileName)
        {

            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        BuffData buff = new BuffData();
                        buff.s_dwID = read.ReadUShort();
                        buff.s_szName = read.ReadZString(Encoding.ASCII, 64 * 2 );
                        buff.s_szComment = read.ReadZString(Encoding.ASCII, 128 * 2);
                        buff.s_nBuffIcon = read.ReadUShort();
                        buff.s_nBuffType = read.ReadUShort();
                        buff.s_nBuffLifeType = read.ReadUShort();
                        buff.s_nBuffTimeType = read.ReadUShort();
                        buff.s_nMinLv = read.ReadUShort();
                        buff.s_nBuffClass = read.ReadUShort();
                        buff.s_dwSkillCode = read.ReadUInt();
                        buff.s_dwDigimonSkillCode = read.ReadInt();
                        buff.s_bDelete = read.ReadByte();
                        buff.s_bDelete1 = read.ReadByte();
                        buff.s_bDelete2 = read.ReadByte();
                        buff.s_szEffectFile = read.ReadZString(Encoding.ASCII, 64);
                        buff.s_nConditionLv = read.ReadUShort();
                        buff.u = read.ReadByte();
                        
                        if (!BuffList.ContainsKey(buff.s_dwID))
                            BuffList.Add(buff.s_dwID, buff);
                    }
                }
            }
            SysCons.LogDB("Buff.bin", "Loaded {0} buffs.", BuffList.Count);
        }


    }

    public class BuffData
    {

        public ushort s_dwID;
        public string s_szName;
        public string s_szComment;
        public ushort s_nBuffIcon;
        public ushort s_nBuffType;
        public ushort s_nBuffLifeType;
        public ushort s_nBuffTimeType;
        public ushort s_nMinLv;
        public ushort s_nBuffClass;
        public uint s_dwSkillCode;
        public int s_dwDigimonSkillCode;
        public byte s_bDelete;
        public byte s_bDelete1;
        public byte s_bDelete2;
        public byte s_bDelete3;
        public string s_szEffectFile;
        public ushort s_nConditionLv;
        public byte u;
      


    }
}